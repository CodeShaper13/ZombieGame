using System;
using UnityEngine.Networking;

public class NetHandlerClient : NetHandlerBase {

    private Player player;

    public NetHandlerClient(Player localPlayer) {
        this.player = localPlayer;
    }

    protected override void registerHandlers() {
        this.func01<MessageShowAnnouncement>();
        this.func01<MessageChangeGameState>();
    }

    public void showAnnouncement(MessageShowAnnouncement msg) {
        this.player.showAnnouncement(msg.message, msg.duration);
    }

    public void changeGameState(MessageChangeGameState msg) {
        Player.localPlayer.setGameState(msg.newState);
    }

    private void func01<T>() where T : AbstractMessage<NetHandlerClient>, new() {
        NetworkManager.singleton.client.RegisterHandler(new T().getID(), delegate (NetworkMessage netMsg) {
            T msg = netMsg.ReadMessage<T>();
            msg.processMessage(this);
        });
    }
}
