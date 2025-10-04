using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace MapMarkerManager;

#nullable disable

public class Static
{
    public static readonly string MOD_ID = "KoboldRanger.MapMarkerManager";
    public static readonly string MAP_COMPONENT_CODE = "markermanager";

    private static ILogger logger;
    public static ILogger Logger { get => logger; set => logger = value; }

    private static ICoreClientAPI capi;
    public static ICoreClientAPI Capi { get => capi; set => capi = value; }
}
