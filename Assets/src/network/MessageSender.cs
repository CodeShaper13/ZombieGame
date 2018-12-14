using System;
using UnityEngine.Networking;

public class MessageSender {

    private const short MSG_ID_SPAWN = 1003;

    public MessageSender() {

    }

    public void spawnEntity(NetworkClient client, MessageBase message) {
        sendMessage(client, message, MSG_ID_SPAWN);
    }

    private void sendMessage(NetworkClient client, MessageBase message, short messageID) {
        client.Send(messageID, message);
    }

    

    private static void OnServerReadyToBeginMessage(NetworkMessage netMsg) {
        throw new NotImplementedException();
    }
}
