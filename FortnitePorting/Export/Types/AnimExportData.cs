using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;

namespace FortnitePorting.Export.Types;

public class AnimExportData : ExportDataBase
{
    public AnimExportData(string name, UObject asset, FStructFallback[] styles, EAssetType type, EExportTargetType exportType) : base(name, asset, styles, type, exportType)
    {
    }
}