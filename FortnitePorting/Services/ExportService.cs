using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using FortnitePorting.Application;
using FortnitePorting.Controls.Assets;
using FortnitePorting.Export;
using FortnitePorting.Export.Types;
using FortnitePorting.Extensions;
using FortnitePorting.Framework.Controls;
using FortnitePorting.Framework.Extensions;
using FortnitePorting.Framework.Services;
using Newtonsoft.Json;
using Serilog;

namespace FortnitePorting.Services;

public static class ExportService
{
    private static readonly SocketInterface Blender = new(BLENDER_PORT, new Dictionary<string, Action>
    {
        { "Animation_InvalidArmature", () => MessageWindow.Show("Message from Blender Server", "An armature must be selected to import an animation. Please select an armature and try again.") }
    });

    private static readonly SocketInterface Unreal = new(UNREAL_PORT);

    private const int BLENDER_PORT = 24000;
    private const int UNREAL_PORT = 24001;

    public static async Task ExportAsync(List<AssetOptions> exports, EExportTargetType exportType)
    {
        await TaskService.RunAsync(async () =>
        {
            if (exportType is EExportTargetType.Folder)
            {
                exports.ForEach(export => CreateExportData(export.AssetItem.DisplayName, export.AssetItem.Asset, export.GetSelectedStyles(), export.AssetItem.Type, exportType));
                return;
            }

            var exportService = exportType switch
            {
                EExportTargetType.Blender => Blender,
                EExportTargetType.Unreal => Unreal
            };

            if (!exportService.Ping())
            {
                var exportTypeString = exportType.GetDescription();
                MessageWindow.Show($"Failed to Connect to {exportTypeString} Server",
                    $"Please ensure that you have {exportTypeString} open with the latest FortnitePorting plugin enabled.");
                return;
            }

            var exportDatas = exports.Select(export => CreateExportData(export.AssetItem.DisplayName, export.AssetItem.Asset, export.GetSelectedStyles(), export.AssetItem.Type, exportType)).ToArray();
            foreach (var exportData in exportDatas) exportData.WaitForExports();

            var exportResponse = CreateExportResponse(exportDatas, exportType);
            exportService.SendData(JsonConvert.SerializeObject(exportResponse));
        });
    }

    public static async Task ExportAsync(List<KeyValuePair<UObject, EAssetType>> assets, EExportTargetType exportType)
    {
        await TaskService.RunAsync(async () =>
        {
            if (exportType is EExportTargetType.Folder)
            {
                assets.ForEach(kvp => CreateExportData(kvp.Key.Name, kvp.Key, Array.Empty<FStructFallback>(), kvp.Value, exportType));
                return;
            }

            var exportService = exportType switch
            {
                EExportTargetType.Blender => Blender,
                EExportTargetType.Unreal => Unreal
            };

            if (!exportService.Ping())
            {
                var exportTypeString = exportType.GetDescription();
                MessageWindow.Show($"Failed to Connect to {exportTypeString} Server",
                    $"Please ensure that you have {exportTypeString} open with the latest FortnitePorting plugin enabled.");
                return;
            }

            var exportDatas = assets.Select(kvp => CreateExportData(kvp.Key.Name, kvp.Key, Array.Empty<FStructFallback>(), kvp.Value, exportType)).ToArray();
            foreach (var exportData in exportDatas) exportData.WaitForExports();

            var exportResponse = CreateExportResponse(exportDatas, exportType);
            exportService.SendData(JsonConvert.SerializeObject(exportResponse));
        });
    }

    private static ExportResponse CreateExportResponse(ExportDataBase[] exportData, EExportTargetType exportType)
    {
        return new ExportResponse
        {
            AssetsFolder = AppSettings.Current.GetExportPath(),
            Options = AppSettings.Current.ExportOptions.Get(exportType),
            Data = exportData
        };
    }

    private static ExportDataBase CreateExportData(string name, UObject asset, FStructFallback[] styles, EAssetType assetType, EExportTargetType exportType)
    {
        return assetType.GetExportType() switch
        {
            EExportType.Mesh => new MeshExportData(name, asset, styles, assetType, exportType),
            EExportType.Animation => new AnimExportData(name, asset, styles, assetType, exportType),
            EExportType.Texture => new TextureExportData(name, asset, styles, assetType, exportType),
            _ => throw new ArgumentOutOfRangeException(assetType.ToString())
        };
    }
}

public class SocketInterface
{
    private UdpClient Client = new();
    private IPEndPoint EndPoint;
    private Dictionary<string, Action> Commands;

    private const string COMMAND_START = "Start";
    private const string COMMAND_STOP = "Stop";
    private const string COMMAND_PING_REQUEST = "Ping";
    private const string COMMAND_PING_RESPONSE = "Pong";
    private const int BUFFER_SIZE = 4096;

    public SocketInterface(int port, Dictionary<string, Action>? commands = null)
    {
        Commands = commands ?? new Dictionary<string, Action>();

        EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        Client.Connect(EndPoint);
    }
    
    public void SendData(string str)
    {
        Client.Send(COMMAND_START.StringToBytes());
        SendSpliced(str.StringToBytes(), BUFFER_SIZE);
        Client.Send(COMMAND_STOP.StringToBytes());
    }

    public bool Ping()
    {
        Client.Send(COMMAND_PING_REQUEST.StringToBytes());
        return TryReceive(out var response) && response.BytesToString().Equals(COMMAND_PING_RESPONSE);
    }

    public int SendSpliced(IEnumerable<byte> arr, int size)
    {
        var chunks = arr.Chunk(size).ToList();

        var dataSent = 0;
        foreach (var (index, chunk) in chunks.Enumerate())
        {
            var tries = 0;
            var chunkSize = Client.Send(chunk);
            while (!Ping())
            {
                Log.Warning("Lost Chunk {Index}, Retrying...", index);
                chunkSize = Client.Send(chunk);

                if (tries > 25) throw new Exception($"Failed to send chunk {index}, data will not continue.");
                tries++;
            }

            dataSent += chunkSize;
        }

        return dataSent;
    }

    private bool TryReceive(out byte[] data)
    {
        data = Array.Empty<byte>();
        try
        {
            data = Client.Receive(ref EndPoint);
        }
        catch (SocketException)
        {
            Client.Close();
            Client = new UdpClient();
            Client.Connect(EndPoint);
            return false;
        }

        return true;
    }
}