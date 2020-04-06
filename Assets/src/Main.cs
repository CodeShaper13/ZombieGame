using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public class Main : MonoBehaviour {

    /// <summary> If true, debug mode is on. </summary>
    public static bool DEBUG
        #if UNITY_EDITOR
        = false
        #endif
        ;
    /// <summary> If true, a new world is instantly created on startup. </summary>
    public const bool DEBUG_FAST_LOAD = true;

    public static bool DEBUG_HEALTH = false;
    public static string SAVE_DIR = "saves/";

    private static Main singleton;

    private GameState gameState;
    private string username;

    public static Main instance() {
        return Main.singleton;
    }

    private void Awake() {
        if (Main.singleton == null) {
            Main.singleton = this;

            NetworkTransport.Init();

            // Preform bootstrap.
            Constants.bootstrap();
            Registry.registryBootstrap();
            GuiManager.guiBootstrap();
            TaskManager.bootstrap();

            // Find a username.
            this.username = this.readUsernameFromFile();
            Logger.log("Setting Username to \"" + this.username + "\"");

            this.gameState = new GameState();
            this.gameState.readFromFile();

            GameObject.DontDestroyOnLoad(gameObject);
        }
        else if (Main.singleton != this) {
            // As every scene contains a Main object, destroy the new ones that are loaded.
            GameObject.Destroy(this.gameObject);
            return;
        }
    }

    public string getUsername() {
        return this.username;
    }

    /// <summary>
    /// Pauses the game and handles the blocking of input.
    /// </summary>
    public void pauseGame() {
        Pause.pause();
        if(Main.getNetManager().isSinglePlayer) {
            Pause.pause();
        }
        GuiManager.openGui(GuiManager.paused);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void resumeGame() {
        GuiManager.closeTopGui();
        Pause.unPause();
    }

    public static void loadScene(string sceneName, MapData data, bool singlePlayer) {
        CustomNetworkManager cnm = Main.getNetManager();
        cnm.isSinglePlayer = singlePlayer;

        cnm.StartHost();
        cnm.ServerChangeScene(sceneName);

        cnm.mapData = data;
    }

    public static CustomNetworkManager getNetManager() {
        return (CustomNetworkManager)NetworkManager.singleton;
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
        Logger.logWarning("Could not find \"username.txt\", generating random username...");
        return Guid.NewGuid().ToString();
    }
}