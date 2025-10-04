using MapMarkerManager.Patches;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace MapMarkerManager.UI;

#nullable disable

public class MarkerManagerMapLayer : MapLayer
{
    public override string Title => "Manage player map markers";
    public override string LayerGroupCode => Static.MAP_COMPONENT_CODE;
    public override EnumMapAppSide DataSide => EnumMapAppSide.Client;

    private readonly string[] waypointKeys = ["Title", "Icon", "Color", "Pinned"];
    private string chosenWaypointKey;
    private GuiElementDynamicText markerListTextHandle;
    private readonly ICoreClientAPI capi;

    public MarkerManagerMapLayer(ICoreAPI api, IWorldMapManager mapSink) : base(api, mapSink)
    {
        capi = api as ICoreClientAPI;
    }

    public override void ComposeDialogExtras(GuiDialogWorldMap guiDialogWorldMap, GuiComposer compo)
    {
        string key = "worldmap-layer-" + LayerGroupCode;

        ElementBounds dlgBounds = ElementStdBounds.AutosizedMainDialog
            .WithFixedPosition(
                    (compo.Bounds.renderX + compo.Bounds.OuterWidth) / RuntimeEnv.GUIScale + 10,
                    (compo.Bounds.renderY + compo.Bounds.OuterHeight) / RuntimeEnv.GUIScale - compo.Bounds.OuterHeight // TODO: dial it in so bottom aligns with map window
                    )
            .WithAlignment(EnumDialogArea.None);

        ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
        bgBounds.BothSizing = ElementSizing.FitToChildren;

        string dynamicTextKey = "markerlist";

        var markerComponent = capi.Gui.CreateCompo(key, dlgBounds)
            .AddShadedDialogBG(bgBounds, false)
            .AddDialogTitleBar(Lang.Get(Static.MAP_COMPONENT_CODE + "-title"), () => { guiDialogWorldMap.Composers[key].Enabled = false; })
            .BeginChildElements(bgBounds)
            .AddDropDown(waypointKeys, waypointKeys, Math.Max(0, Array.IndexOf(waypointKeys, chosenWaypointKey)), OnSelectionChanged, ElementBounds.Fixed(0, 30, 160, 35))
            .AddDynamicText("", CairoFont.WhiteDetailText(), ElementBounds.Fixed(0, 80, 160, 300), dynamicTextKey)
            .EndChildElements()
            .Compose();

        guiDialogWorldMap.Composers[key] = markerComponent;

        markerListTextHandle = markerComponent.GetDynamicText(dynamicTextKey);
    }

    private void OnSelectionChanged(string code, bool selected)
    {
        chosenWaypointKey = code;
        BuildWaypointList();
    }

    private string WaypointPropValueAsString(Waypoint waypoint)
    {
        return chosenWaypointKey switch
        {
            "Title" => waypoint.Title,
            "Icon" => waypoint.Icon,
            "Color" => waypoint.Color.ToString(),
            "Pinned" => waypoint.Pinned.ToString(),
            _ => throw new Exception("Don't know what to do with ident: " + chosenWaypointKey),
        };
    }

    private void BuildWaypointList()
    {
        if (WaypointMapLayerPatcher.WaypointList == null)
        {
            markerListTextHandle.SetNewTextAsync("");
            return;
        }

        List<string> waypointIdentList = WaypointMapLayerPatcher.WaypointList
            .Aggregate([], (List<string> accum, Waypoint waypoint) =>
            {
                string newListEntry = WaypointPropValueAsString(waypoint);
                if (!string.IsNullOrEmpty(newListEntry))
                {
                    accum.Add(newListEntry);
                }
                return accum;
            });

        if (waypointIdentList.Count == 0)
        {
            markerListTextHandle.SetNewTextAsync("");
            return;
        }

        markerListTextHandle.SetNewTextAsync(waypointIdentList?.Aggregate((accum, waypointIdent) =>
                        {
                            return accum + "\n" + waypointIdent;
                        }) ?? "");
    }
}
