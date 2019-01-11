public class GuiManager {

    public static GuiBase paused;
    public static GuiBase unitStats;

    /// <summary>
    /// The gui that is currently open.
    /// </summary>
    public static GuiBase currentGui;

    /// <summary>
    /// Opens the passed gui, closing any previously open ones.
    /// </summary>
    public static GuiBase openGui(GuiBase newGui) {
        if(GuiManager.currentGui != null) {
            // Hide the current gui screen, only if there is one.
            // In the event of pressing pause while playing or when opening the
            // title screen there is no current gui.
            GuiManager.currentGui.gameObject.SetActive(false);
        }
        GuiManager.currentGui = newGui;

        if(GuiManager.currentGui != null) {
            GuiManager.currentGui.gameObject.SetActive(true);
        }

        return newGui;
    }

    public static GuiBase getGuiFromId(int guidId) {
        switch(guidId) {
            case 0: return GuiManager.paused;
            case 1: return GuiManager.unitStats;
            default: return null;
        }
    }

    /// <summary>
    /// Closes the current gui, hiding it.
    /// </summary>
    public static void closeCurrentGui() {
        if(GuiManager.currentGui != null) {
            GuiManager.currentGui.gameObject.SetActive(false);
            GuiManager.currentGui = null;
        }
    }

    public static void guiBootstrap() {
        GuiManager.paused = References.list.guiPausedObject;
        GuiManager.unitStats = References.list.guiUnitStatsObject;
    }
}