﻿using UnityEngine;

public class Main : MonoBehaviour {

    /// <summary> If true, debug mode is on. </summary>
    public static bool DEBUG = true;
    public static bool DEBUG_HEALTH = false;

    private static Main singleton;

    private bool paused;

    public static Main instance() {
        return Main.singleton;
    }

    private void Awake() {
        if (Main.singleton == null) {
            Main.singleton = this;

            // Preform bootstrap.
            Constants.bootstrap();
            Registry.registryBootstrap();
            Names.bootstrap();
        }
        else if (singleton != this) {
            // As every scene contains a Main object, destroy the new ones that are loaded.
            GameObject.Destroy(this.gameObject);
            return;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Returns true if the game is paused.
    /// </summary>
    public bool isPaused() {
        return this.paused;
    }

    /// <summary>
    /// Pauses the game and handles the blocking of input.
    /// </summary>
    public void pauseGame() {
        this.paused = true;
        Time.timeScale = 0;
//        GuiManager.openGui(GuiManager.paused);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void resumeGame() {
//        GuiManager.closeCurrentGui();
        this.paused = false;
        Time.timeScale = 1;
    }
}