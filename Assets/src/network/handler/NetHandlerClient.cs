using UnityEngine;
using UnityEngine.Networking;

public class NetHandlerClient : NetHandlerBase {

    private Player player;

    public NetHandlerClient(Player localPlayer) {
        this.player = localPlayer;
    }

    protected override void registerHandlers() {
        this.registerMsg<MessageShowAnnouncement>();
        this.registerMsg<MessageChangeGameState>();
        this.registerMsg<MessageShowStatsGui>();
    }

    public void showAnnouncement(MessageShowAnnouncement msg) {
        this.player.showAnnouncement(msg.message, msg.duration);
    }

    public void changeGameState(MessageChangeGameState msg) {
        Player.localPlayer.setGameState(msg.newState);
    }

    public void showStatsGui(MessageShowStatsGui msg) {
        GuiUnitStats gui = (GuiUnitStats)GuiManager.openGui(GuiManager.unitStats);
        gui.set(msg);
        Debug.Log("123");
    }

    private void registerMsg<T>() where T : AbstractMessageClient, new() {
        NetworkManager.singleton.client.RegisterHandler(new T().getID(), delegate (NetworkMessage netMsg) {
            T msg = netMsg.ReadMessage<T>();
            msg.processMessage(this);
        });
    }
}
