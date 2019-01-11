using System;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour {

    /// <summary> If true, debug mode is on. </summary>
    public static bool DEBUG
        #if UNITY_EDITOR
        = true
        #endif
        ;
    public static bool DEBUG_HEALTH = false;

    private static Main singleton;

    private string username;
    private bool paused;

    /// <summary> If true, the current game is a single player game. 
    /// This is not accurate outside of a game (eg, on the title scree).
    /// </summary>
    public bool isSinglePlayerGame;

    public static Main instance() {
        return Main.singleton;
    }

    private void Awake() {
        if (Main.singleton == null) {
            Main.singleton = this;

            // Preform bootstrap.
            Constants.bootstrap();
            Registry.registryBootstrap();
            GuiManager.guiBootstrap();
            Names.bootstrap();
            TaskManager.bootstrap();

            // Find a username.
            this.username = this.readUsernameFromFile();
            Logger.log("Setting Username to \"" + this.username + "\"");

            #if UNITY_EDITOR
                // Debug
                this.isSinglePlayerGame = GameObject.FindObjectOfType<CustomNetworkManager>() == null;
            #endif
        }
        else if (Main.singleton != this) {
            // As every scene contains a Main object, destroy the new ones that are loaded.
            GameObject.Destroy(this.gameObject);
            return;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }

    public string getUsername() {
        return this.username;
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
        GuiManager.openGui(GuiManager.paused);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void resumeGame() {
        GuiManager.closeCurrentGui();
        this.paused = false;
        Time.timeScale = 1;
    }

    private string readUsernameFromFile() {
        string path = "username.txt";
        if(File.Exists(path)) {
            string[] lines = File.ReadAllLines(path);
            if(lines.Length > 0) {
                string s1 = lines[0].Replace(' ', '_');
                return s1;
            }
        }
        Logger.logWarning("Could not find \"username.txt\", generating random username.");
        return Guid.NewGuid().ToString();
    }
}