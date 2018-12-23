using UnityEngine;

public class MessageSetObjectPostion : AbstractMessage<NetHandlerServer> {

    public readonly GameObject target;
    public readonly Vector3 newPosition;

    public MessageSetObjectPostion() { }

    public MessageSetObjectPostion(GameObject target, Vector3 newPosition) {
        this.target = target;
        this.newPosition = newPosition;
    }

    public override short getID() {
        return 1003;
    }

    public override void processMessage(NetHandlerServer handler) {
        handler.moveObject(this);
    }
}
