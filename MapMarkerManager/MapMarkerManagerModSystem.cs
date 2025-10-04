using HarmonyLib;
using MapMarkerManager.UI;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace MapMarkerManager;

#nullable disable

public class MapMarkerManagerModSystem : ModSystem
{
    private Harmony harmony;

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
