using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GuiPauseScreen : GuiBase {

    [SerializeField]
    private Button saveGameButton;

    public override void onGuiInit() {
        this.saveGameButton.gameObject.SetActive(false);
    }

    public override void onGuiOpen() {
        if(Map.instance.isServer) {
            this.saveGameButton.gameObject.SetActive(true);
        }
    }

    public override void onGuiClose() {
        // Hide buttons, they will be reactivated when the gui opens.
        this.saveGameButton.gameObject.SetActive(false);

        Main.instance().resumeGame();
    }

    public void callback_resume() {
        Main.instance().resumeGame();
    }

    public void callback_saveGame() {
        Map.instance.saveMap();
    }

    public void callback_quitGame() {
        NetworkManager manager = NetworkManager.singleton;

        if(NetworkServer.active || manager.IsClientConnected()) {
            manager.StopHost();
        }

        GuiManager.closeCurrentGui();
    }
}
