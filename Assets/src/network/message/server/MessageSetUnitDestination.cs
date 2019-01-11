using UnityEngine;

public class MessageSetUnitDestination : AbstractMessageServer {

    public readonly GameObject obj;
    public readonly Vector3 destination;
    public readonly int partySize;

    public MessageSetUnitDestination() { }

    public MessageSetUnitDestination(UnitBase unit, Vector3 destination, int partySize) {
        this.obj = unit.gameObject;
        this.destination = destination;
        this.partySize = partySize;
    }

    public override short getID() {
        return 1005;
    }

    public override void processMessage(ConnectedPlayer sender, NetHandlerServer handler) {
        handler.func02(this);
    }
}
