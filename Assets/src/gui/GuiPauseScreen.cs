using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuiPauseScreen : GuiBase {

    [SerializeField]
    private Text text;
    [SerializeField]
    private Button saveGameButton;
    [SerializeField]
    private Button broadcastButton;
    [SerializeField]
    private Button restartButton;

    public override void onGuiInit() { }

    public override void onGuiOpen() {
        if(Main.getNetManager().isSinglePlayer) {
            // Single Player
            this.text.text = "Paused";
            this.restartButton.gameObject.SetActive(true);
            this.saveGameButton.gameObject.SetActive(false);
            this.broadcastButton.gameObject.SetActive(false);
        }
        else {
            // Multi Player
            this.text.text = string.Empty;
            this.restartButton.gameObject.SetActive(false);

            if(MapBase.instance.isServer) {
                // Host
                this.saveGameButton.gameObject.SetActive(true);
                this.broadcastButton.gameObject.SetActive(true);

                if(GameObject.FindObjectOfType<NetworkDiscovery>().running) {
                    this.broadcastButton.interactable = false;
                }
            } else {
                this.saveGameButton.gameObject.SetActive(true);
            }
        }
    }

    public override void onGuiClose() {
        Main.instance().resumeGame();
    }

    public void callback_resume() {
        Main.instance().resumeGame();
    }

    public void callback_saveGame() {
        ((MapMP)MapBase.instance).saveMap();
    }

    public void callback_quitGame() {
        NetworkManager manager = NetworkManager.singleton;

        if(NetworkServer.active || manager.IsClientConnected()) {
            print("stopping");
            manager.StopHost();
        }

        GameObject.FindObjectOfType<NetworkDiscovery>().StopBroadcast();

        SceneManager.LoadScene("TitleScreen");

        GuiManager.closeAllGui();
    }

    public void callback_broadcast() {
        NetworkDiscovery discovery = GameObject.FindObjectOfType<NetworkDiscovery>();
        discovery.Initialize();
        //discovery.broadcastData = "spam!";  // This is NetworkManager:localhost:7777 by default
        discovery.StartAsServer();

        this.broadcastButton.interactable = false;
    }

    public void callback_restart() {
        CampaignLevelData cld = ((MapSP)MapBase.instance).getCampaignData();
        // TODO load scene again
    }
}
