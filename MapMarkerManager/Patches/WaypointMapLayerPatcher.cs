using System.Collections.Generic;
using HarmonyLib;
using MapMarkerManager.UI;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace MapMarkerManager.Patches;

[HarmonyPatch(typeof(WaypointMapLayer))]
public class WaypointMapLayerPatcher
{
    public static Dictionary<string, List<Waypoint>> WaypointsByName { get; private set; }
    public static Dictionary<string, List<Waypoint>> WaypointsByIcon { get; private set; }
    public static Dictionary<int, List<Waypoint>> WaypointsByColour { get; private set; }
    public static Dictionary<bool, List<Waypoint>> WaypointsByPinned { get; private set; }

    private static void ResetDicts()
    {
        WaypointsByName = [];
        WaypointsByIcon = [];
        WaypointsByColour = [];
        WaypointsByPinned = [];
    }

    private static void AddToDict<KeyType>(Dictionary<KeyType, List<Waypoint>> dict, KeyType key, Waypoint newVal) where KeyType : notnull
    {
        if (dict.TryGetValue(key, out List<Waypoint>? value))
        {
            value.Add(newVal);
        }
        else
        {
            dict[key] = [newVal];
        }
    }

    // NOTE: might be overkill? may be better to build waypoints list once,
    // and patch functions that add/remove waypoints to keep ourselves in sync.
    // but this way everything should always be in sync at least
    [HarmonyPostfix]
    [HarmonyPatch(nameof(WaypointMapLayer.OnDataFromServer))]
    public static void Postfix_OnDataFromServer(WaypointMapLayer __instance)
    {
        ResetDicts();
        Static.Logger.Notification("Have " + __instance.ownWaypoints.Count + " total waypoints");

        foreach (Waypoint waypoint in __instance.ownWaypoints)
        {
            AddToDict(WaypointsByName, waypoint.Title, waypoint);
            AddToDict(WaypointsByIcon, waypoint.Icon, waypoint);
            AddToDict(WaypointsByColour, waypoint.Color, waypoint);
            AddToDict(WaypointsByPinned, waypoint.Pinned, waypoint);
        }

        Static.Logger.Notification("Have " + WaypointsByName.Count + " name categories");
        Static.Logger.Notification("Have " + WaypointsByColour.Count + " icon categories");
        Static.Logger.Notification("Have " + WaypointsByIcon.Count + " colour categories");
        Static.Logger.Notification("Have " + WaypointsByPinned.Count + " pinned categories");
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
