using HarmonyLib;
using System.Collections.Generic;
using Vintagestory.GameContent;

namespace MapMarkerManager.Patches;

#nullable disable

[HarmonyPatch(typeof(WaypointMapLayer))]
public class WaypointMapLayerPatcher
{
    public static List<Waypoint> WaypointList { get; private set; }

    private static void AddToDict<KeyType>(Dictionary<KeyType, List<Waypoint>> dict, KeyType key, Waypoint newVal) where KeyType : notnull
    {
        if (dict.TryGetValue(key, out List<Waypoint> value))
        {
            value.Add(newVal);
        }
        else
        {
            dict[key] = [newVal];
        }
    }

    // NOTE: may be better to listen to waypoint added and removed events (if possible) and adjust from there
    // but we need to get an initial list either way, so maybe this is fine?
    [HarmonyPostfix]
    [HarmonyPatch(nameof(WaypointMapLayer.OnDataFromServer))]
    public static void Postfix_OnDataFromServer(WaypointMapLayer __instance)
    {
        WaypointList = __instance.ownWaypoints;
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(nameof(WaypointMapLayer.OnMapOpenedClient))]
    // public static void Postfix_OnMapOpenedClient()
    // {
    //     Static.Logger.Notification("Trying to open the hanger-on");
    //     // MarkerListPrev._instance.TryOpen();
    // }
    //
    // [HarmonyPostfix]
    // [HarmonyPatch(nameof(WaypointMapLayer.OnMapClosedClient))]
    // public static void Postfix_OnMapClosedClient()
    // {
    //     Static.Logger.Notification("Trying to close the hanger-on");
    //     // MarkerListPrev._instance.TryClose();
    // }
}
