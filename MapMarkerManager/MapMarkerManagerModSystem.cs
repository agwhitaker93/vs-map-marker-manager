using HarmonyLib;
using MapMarkerManager.UI;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace MapMarkerManager;

// TODO: look into how ModSystemOreMap works, I think it stores data on the server.
// Might be worth looking into to move away from constantly rebuilding the waypoint list/s
public class MapMarkerManagerModSystem : ModSystem
{
    private Harmony harmony;

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
    {
        Static.Capi = api;
        Static.Logger = Mod.Logger;

        harmony = new Harmony(Static.MOD_ID);
        harmony.PatchAll();

        api.ModLoader.GetModSystem<WorldMapManager>().RegisterMapLayer<MarkerManagerMapLayer>(Static.MAP_COMPONENT_CODE, 2);
    }

    public override void Dispose()
    {
        harmony.UnpatchAll(Static.MOD_ID);
    }
}
