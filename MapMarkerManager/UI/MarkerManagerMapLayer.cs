using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace MapMarkerManager.UI;

public class MarkerManagerMapLayer : MapLayer
{
    public override string Title => "Manage player map markers";
    public override string LayerGroupCode => Static.MAP_COMPONENT_CODE;
    public override EnumMapAppSide DataSide => EnumMapAppSide.Client;

    private ICoreClientAPI capi;

    public MarkerManagerMapLayer(ICoreAPI api, IWorldMapManager mapSink) : base(api, mapSink)
    {
        capi = api as ICoreClientAPI;
    }

    public override void ComposeDialogExtras(GuiDialogWorldMap guiDialogWorldMap, GuiComposer compo)
    {
        Static.Logger.Notification("Composing marker manager map layer dialog extras");

        string key = "worldmap-layer-" + LayerGroupCode;

        // Auto-sized dialog at the center of the screen
        ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);

        // Just a simple 300x100 pixel box with 40 pixels top spacing for the title bar
        ElementBounds textBounds = ElementBounds.Fixed(0, 40, 300, 100);

        // Background boundaries. Again, just make it fit it's child elements, then add the text as a child element
        ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
        bgBounds.BothSizing = ElementSizing.FitToChildren;
        bgBounds.WithChildren(textBounds);

        guiDialogWorldMap.Composers[key] = capi.Gui.CreateCompo(key, dialogBounds)
            .AddShadedDialogBG(bgBounds)
            .AddDialogTitleBar("Heck yeah!", () => { guiDialogWorldMap.Composers[key].Enabled = false; })
            .AddStaticText("This is a piece of text at the center of your screen - Enjoy!", CairoFont.WhiteDetailText(), textBounds)
            .Compose();
    }

    public override void OnMapOpenedClient()
    {
        Static.Logger.Notification("Map opened!");
    }

    public override void OnMapClosedClient()
    {
        Static.Logger.Notification("Map closed!");
    }
}
