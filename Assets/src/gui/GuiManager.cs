using System;
using System.Collections.Generic;

public class GuiManager {

    #region GUI_References
    // Gameplay GUIs.
    public static GuiBase paused;
    public static GuiBase unitStats;

    // Town GUIs.
    public static GuiBase campaignSelect;
    #endregion

    public static Stack<GuiBase> currentGui;

    /// <summary>
    /// Opens the passed gui, closing any previously open ones.
    /// </summary>
    public static GuiBase openGui(GuiBase newGui) {
        debugAssert(newGui);

        GuiManager.closeTopGui();

        return func(newGui);
    }

    public static GuiBase openGuiAdditively(GuiBase newGui) {
        debugAssert(newGui);

        return func(newGui);
    }

    private static GuiBase func(GuiBase newGui) {
        GuiManager.currentGui.Push(newGui);

        newGui.gameObject.SetActive(true);

        return newGui;
    }

    private static void debugAssert(GuiBase newGui) {
        if(newGui == null) {
            throw new Exception("Null was passed while trying to open a GUI!");
        }
    }

    // Unused?
    public static GuiBase getGuiFromId(int guidId) {
        switch(guidId) {
            case 0: return GuiManager.paused;
            case 1: return GuiManager.unitStats;
            default: return null;
        }
    }

    /// <summary>
    /// Closes the top level GUI.
    /// </summary>
    public static void closeTopGui() {
        if(GuiManager.currentGui.Count != 0) {
            GuiBase gui = GuiManager.currentGui.Pop();
            if(gui != null) {
                gui.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Closes all GUIs in the reverse order that they were opened in.
    /// </summary>
    public static void closeAllGui() {
        while(GuiManager.currentGui.Count != 0) {
            GuiManager.closeTopGui();
        }
    }

    public static void guiBootstrap() {
        GuiManager.currentGui = new Stack<GuiBase>();

        GuiManager.paused = References.list.guiPausedObject;
        GuiManager.unitStats = References.list.guiUnitStatsObject;
        GuiManager.campaignSelect = References.list.guiCampaignSelect;
    }
}