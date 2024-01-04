using System.ComponentModel;
using FortnitePorting.Extensions;

namespace FortnitePorting;

public enum ELoadingType
{
    [Description("Local (Installed)")]
    Local,

    [Description("Live (On-Demand)")]
    Live,

    [Description("Custom (Old Versions)")]
    Custom
}

public enum EAssetType
{
    [Description("None")]
    None,
    
    // COSMETIC
    
    [Description("Outfits"), Export(EExportType.Mesh)]
    Outfit,
    
    [Description("Lego Outfits"), Export(EExportType.Mesh)]
    LegoOutfit,

    [Description("Back Blings"), Export(EExportType.Mesh)]
    Backpack,

    [Description("Pickaxes"), Export(EExportType.Mesh)]
    Pickaxe,

    [Description("Gliders"), Export(EExportType.Mesh)]
    Glider,

    [Description("Pets"), Export(EExportType.Mesh)]
    Pet,

    [Description("Toys"), Export(EExportType.Mesh)]
    Toy,

    [Description("Emoticons"), Export(EExportType.Texture)]
    Emoticon,

    [Description("Sprays"), Export(EExportType.Texture)]
    Spray,

    [Description("Banners"), Export(EExportType.Texture)]
    Banner,

    [Description("Loading Screens"), Export(EExportType.Texture)]
    LoadingScreen,

    [Description("Emotes"), Export(EExportType.Animation)]
    Emote,
    
    // CREATIVE

    [Description("Props"), Export(EExportType.Mesh)]
    Prop,

    [Description("Prefabs"), Export(EExportType.Mesh)]
    Prefab,
    
    // GAMEPLAY

    [Description("Items"), Export(EExportType.Mesh)]
    Item,
    
    [Description("Resources"), Export(EExportType.Mesh)]
    Resource,

    [Description("Traps"), Export(EExportType.Mesh)]
    Trap,

    [Description("Vehicles"), Export(EExportType.Mesh)]
    Vehicle,

    [Description("Wildlife"), Export(EExportType.Mesh)]
    Wildlife,
    
    // FESTIVAL
    
    [Description("Guitars"), Export(EExportType.Mesh)]
    FestivalGuitar,
    
    [Description("Basses"), Export(EExportType.Mesh)]
    FestivalBass,
    
    [Description("Keytars"), Export(EExportType.Mesh)]
    FestivalKeytar,
    
    [Description("Drums"), Export(EExportType.Mesh)]
    FestivalDrum,
    
    [Description("Microphones"), Export(EExportType.Mesh)]
    FestivalMic,
    
    // GENERIC

    [Description("Mesh"), Export(EExportType.Mesh)]
    Mesh,
    
    [Description("World"), Export(EExportType.Mesh)]
    World,
    
    [Description("Texture"), Export(EExportType.Texture)]
    Texture,
    
    [Description("Animation"), Export(EExportType.Animation)]
    Animation
    
}

public enum EExportType
{
    [Description("Mesh")]
    Mesh,

    [Description("Animation")]
    Animation,

    [Description("Texture")]
    Texture
}


public enum EExportTargetType
{
    [Description("Blender")]
    Blender,

    [Description("Unreal Engine")]
    Unreal,

    [Description("Folder")]
    Folder
}

public enum ESortType
{
    [Description("Default")]
    Default,

    [Description("A-Z")]
    AZ,

    [Description("Season")]
    Season,

    [Description("Rarity")]
    Rarity
}

public enum EImageType
{
    [Description("PNG (.png)")]
    PNG,

    [Description("Targa (.tga)")]
    TGA
}

public enum ERigType
{
    [Description("Default Rig (FK)")]
    Default,

    [Description("Tasty Rig (IK)")]
    Tasty
}

public enum ESupportedLODs
{
    [Description("LOD 0")]
    LOD0,

    [Description("LOD 1")]
    LOD1,

    [Description("LOD 2")]
    LOD2,

    [Description("LOD 3")]
    LOD3,

    [Description("LOD 4")]
    LOD4
}

public enum EMeshExportTypes
{
    [Description(".uemodel")]
    UEFormat,

    [Description(".psk")]
    ActorX
}

public enum EAnimExportTypes
{
    [Description(".ueanim")]
    UEFormat,

    [Description(".psa")]
    ActorX
}