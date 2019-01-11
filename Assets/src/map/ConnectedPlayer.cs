using UnityEngine;
using UnityEngine.Networking;

public class ConnectedPlayer {

    private Player playerObject;
    private NetworkConnection connection;

    public ConnectedPlayer(NetworkConnection connection, Player player) {
        this.connection = connection;
        this.playerObject = player;
    }

    /// <summary>
    /// Returns the team that this player controlls.
    /// </summary>
    public Team getTeam() {
        return this.playerObject.team;
    }

    public NetworkConnection getConnection() {
        return this.connection;
    }

    public Player getPlayer() {
        return this.playerObject;
    }

    /// <summary>
    /// Sends a message to this player.
    /// </summary>
    public void sendMessage(AbstractMessageClient msg) {
        this.connection.Send(msg.getID(), msg);
    }
}
