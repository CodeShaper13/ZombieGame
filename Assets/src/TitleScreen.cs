using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public class TitleScreen : MonoBehaviour {

    public CampaignLevelData cld;

    [SerializeField]
    private NetworkDiscovery discovery;
    private int state = 0;

    private static bool initialLoad;

    private void Start() {
        /*
        initialLoad = true;
        this.loadSpWorld(this.cld);
        return;
        */

        #if UNITY_EDITOR
            if(!initialLoad && Main.DEBUG_FAST_LOAD) {
            initialLoad = true;
                MapData data = new MapData("123", 111599);
                Logger.log("Instantly Loading a world, Debug mode must be on and we're in the Editor...");
                this.loadMpWorld(data);
            }
        #endif
    }

    private void OnGUI() {
        switch(this.state) {
            case -1: break;
            case 0: this.drawMain(); break;
            case 1: this.drawNewMap(); break;
            case 2: this.drawLoadMap(); break;
            case 3: this.drawJoinGame(); break;
        }
    }

    private void drawMain() {
        float w = 250;
        float h = 80;

        if(GUI.Button(new Rect(20, 20, w, h), "New Game")) {
            this.state = 1;
            for(int i = 0; i < int.MaxValue; i++) {
                GameSaver gs = new GameSaver("world" + i);
                if(!gs.doesSaveExists()) {
                    this.worldName = "world" + i;
                    break;
                }
            }
            this.type = -1;
            this.seed = UnityEngine.Random.Range(int.MaxValue, int.MinValue).ToString();
        }
        if(GUI.Button(new Rect(20, 120, w, h), "Load Game")) {
            this.state = 2;
        }
        if(GUI.Button(new Rect(20, 220, w, h), "Join Game")) {
            this.discovery.Initialize();
            this.discovery.StartAsClient();
            this.state = 3;
        }
        if(GUI.Button(new Rect(20, 320, w, h), "Single Player")) {
            this.loadSpWorld(this.cld);
        }
        if(GUI.Button(new Rect(20, 420, w, h), "Exit")) {
            #if UNITY_EDITOR
                Logger.log("Exiting Game...");
            #endif
            Application.Quit();
        }
    }

    private void drawLoadMap() {
        if(!Directory.Exists(Main.SAVE_DIR)) {
            Directory.CreateDirectory(Main.SAVE_DIR);
        }

        string[] allSaves = Directory.GetDirectories(Main.SAVE_DIR);
        List<string> saves = new List<string>();
        foreach(string saveFolder in allSaves) {
            string mapDataPth = saveFolder + "/" + "map.nbt";
            if(File.Exists(mapDataPth)) {
                saves.Add(saveFolder.Substring(saveFolder.IndexOf('/') + 1));
            }
        }

        float f = 25;
        foreach(string s in saves) {
            if(GUI.Button(new Rect(10, f, 250, 60), "Load: " + s)) {
                GameSaver gameSaver = new GameSaver(s);

                MapData data = gameSaver.readMapDataFromFile();
                this.loadScene("Forest", data, false);
            }
            f += 85;
        }

        this.drawBackBtn();
    }

    #region NewWorldSettings
    private string seed;
    private string worldName;
    private int type;
    #endregion

    private void drawNewMap() {
        float w = 250;
        float h = 25;

        GUI.Label(new Rect(10, 10, w, h), "World Name: ");
        this.worldName = GUI.TextArea(new Rect(100, 10, w, h), this.worldName);

        GUI.Label(new Rect(10, 10 + 40, w, h), "Seed: ");
        this.seed = GUI.TextArea(new Rect(100, 10 + 40, w, h), this.seed);

        GUI.Label(new Rect(10, 10 + 80, w, h), "Map Type: ");
        string[] n = new string[] { "Forest", "Mountains", "Plains" };
        for(int i = 0; i < 3; i++) {
            if(GUI.Button(new Rect(10 + 25, 10 + 80 + h + (h * i), 100, h), n[i])) {
                this.type = i;
            }
        }

        if(this.type != -1 && !string.IsNullOrEmpty(this.seed)) {
            if(GUI.Button(new Rect(10, 250, w, h), "Create " + n[this.type] + " World")) {
                MapData data = new MapData(this.worldName, int.Parse(this.seed));
                this.loadMpWorld(data);
            }
        }

        this.drawBackBtn();
    }

    private static string BytesToString(byte[] bytes) {
        char[] chars = new char[bytes.Length / sizeof(char)];
        Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    private void drawJoinGame() {
        GUI.Label(new Rect(20, 20, 100, 100), "Searching for a LAN game...");

        int xpos = 150;
        int ypos = 10;
        if(this.discovery.broadcastsReceived != null) {
            foreach(var addr in this.discovery.broadcastsReceived.Keys) {
                var value = this.discovery.broadcastsReceived[addr];
                if(GUI.Button(new Rect(xpos, ypos + 20, 300, 20), "Game at " + addr) && true) {
                    string dataString = BytesToString(value.broadcastData);
                    string[] items = dataString.Split(':');
                    //print(dataString);
                    //foreach(string s12 in items) {
                    //    print(s12);
                    //}
                    if(items.Length == 3) { // && items[0] == "NetworkManager") { // TODO should we include this?
                        if(NetworkManager.singleton != null && NetworkManager.singleton.client == null) {
                            NetworkManager.singleton.networkAddress = items[1];
                            NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
                            NetworkManager.singleton.StartClient();
                        }
                    }
                }
                ypos += 24;
            }
        }

        GUI.Label(new Rect(20, 250, 250, 25), "Direct Connect");
        if(GUI.Button(new Rect(40, 280, 105, 20), "Connect")) {
            NetworkManager.singleton.StartClient();
        }
        NetworkManager.singleton.networkAddress = GUI.TextField(new Rect(40 + 100, 280, 95, 20), NetworkManager.singleton.networkAddress);

        if(this.drawBackBtn()) {
            this.discovery.StopBroadcast();
        }
    }

    private bool drawBackBtn() {
        bool flag = GUI.Button(new Rect(20, 400, 300, 80), "Back");
        if(flag) {
            this.state = 0;
        }
        return flag;
    }

    private void loadSpWorld(CampaignLevelData campaignData) {
        CustomNetworkManager cnm = Main.getNetManager();
        cnm.isSinglePlayer = true;
        cnm.campaignData = campaignData;

        MapData data = new MapData("nul", 0);
        this.loadScene(campaignData.sceneName, data, true);
    }

    private void loadMpWorld(MapData data) {
        this.loadScene("Lvl1", data, false);
    }

    private void loadScene(string sceneName, MapData data, bool singlePlayer) {
        CustomNetworkManager cnm = Main.getNetManager();
        cnm.isSinglePlayer = singlePlayer;

        NetworkManager.singleton.StartHost();
        NetworkManager.singleton.ServerChangeScene(sceneName);

        cnm.mapData = data;
    }
}
