using UnityEngine;

public class MessageSetUnitDestination : AbstractMessage<NetHandlerServer> {

    public readonly GameObject obj;
    public readonly Vector3 destination;

    public MessageSetUnitDestination() { }

    public MessageSetUnitDestination(UnitBase unit, Vector3 destination) {
        this.obj = unit.gameObject;
        this.destination = destination;
    }

    public override short getID() {
        return 1005;
    }

    public override void processMessage(NetHandlerServer handler) {
        handler.func02(this);
    }
}
