using Vintagestory.API.Client;

namespace MapMarkerManager.UI;

public class MarkerListPrev : GuiDialog
{
    public static GuiDialog _instance;

    public override string ToggleKeyCombinationCode => "toggle-marker-list-gui";

    public static void Init()
    {
        _instance = new MarkerListPrev(Static.Capi);
    }

    public MarkerListPrev(ICoreClientAPI capi) : base(capi)
    {
        // Auto-sized dialog at the center of the screen
        ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);

        // Just a simple 300x100 pixel box with 40 pixels top spacing for the title bar
        ElementBounds textBounds = ElementBounds.Fixed(0, 40, 300, 100);

        // Background boundaries. Again, just make it fit it's child elements, then add the text as a child element
        ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
        bgBounds.BothSizing = ElementSizing.FitToChildren;
        bgBounds.WithChildren(textBounds);

        SingleComposer = capi.Gui.CreateCompo("myAwesomeDialog", dialogBounds)
            .AddShadedDialogBG(bgBounds)
            .AddDialogTitleBar("Heck yeah!", OnTitleBarCloseClicked)
            .AddStaticText("This is a piece of text at the center of your screen - Enjoy!", CairoFont.WhiteDetailText(), textBounds)
            .Compose()
        ;
    }

    private void OnTitleBarCloseClicked()
    {
        TryClose();
    }
}
