using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using HarmonyLib;

namespace MapMarkerManager;

[HarmonyPatch]
public class MapMarkerManagerModSystem : ModSystem
{
    public static readonly string MOD_ID = "KoboldRanger.MapMarkerManager";

    private static Harmony harmony;
    private static ILogger Logger;

    // Called on server and client
    // Useful for registering block/entity classes on both sides
    // public override void Start(ICoreAPI api)
    // {
    //     Mod.Logger.Notification("Hello from template mod: " + api.Side);
    // }
    //
    // public override void StartServerSide(ICoreServerAPI api)
    // {
    //     Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("mapmarkermanager:hello"));
    // }

    public override void StartClientSide(ICoreClientAPI api)
    // public override void StartServerSide(ICoreServerAPI api)
    {
        Logger = Mod.Logger;
        Logger.Notification("Hello from template mod: " + Lang.Get("mapmarkermanager:hello"));

        harmony = new Harmony(MOD_ID);
        harmony.PatchAll();
    }

    public override void Dispose()
    {
        harmony.UnpatchAll(MOD_ID);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WaypointMapLayer), "OnMapOpenedClient")]
    public static void Patch_Postfix_WaypointMapLayer_OnMapOpenedClient(WaypointMapLayer __instance)
    {
        Logger.Notification("Doing waypoint postfix stuff!");
        Logger.Notification("There are " + __instance.ownWaypoints.Count + " waypoints");
        foreach (Waypoint waypoint in __instance.ownWaypoints) {
            Logger.Notification("Got waypoint: \"" + waypoint.Title + "\", " + waypoint.Position);
        }
    }
}
