using UnityEngine;
using UnityEngine.Networking;

public class MyMessage : MessageBase {
    public NetworkInstanceId netId;
    public int stuff;
}

public class Test : NetworkBehaviour {
    short MyMsgId = 1000;

    public override void OnStartClient() {
        // this should be somewhere else..
        NetworkManager.singleton.client.RegisterHandler(MyMsgId, OnMyMsg);
    }

    [Command]
    void CmdSendToMe() {
        var msg = new MyMessage();
        msg.stuff = 2456986;
        msg.netId = netId;

        base.connectionToClient.Send(MyMsgId, msg);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdSendToMe();
        }
    }

    void DoStuff(int stuff) {
        Debug.Log("Got msg " + stuff + " for " + gameObject);
    }

    // this function would be on a manager class
    static void OnMyMsg(NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<MyMessage>();
        var player = ClientScene.FindLocalObject(msg.netId);
        print(msg.stuff);
    }
}