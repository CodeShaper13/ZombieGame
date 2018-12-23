using UnityEngine;

public class MessageRemoveEntity : AbstractMessage<NetHandlerServer> {

    public readonly GameObject gameObj;

    public MessageRemoveEntity() { }

    public MessageRemoveEntity(MapObject mapObj) {
        this.gameObj = mapObj.gameObject;
    }

    public override void processMessage(NetHandlerServer handler) {
        handler.removeEntity(this);
    }

    public override short getID() {
        return 1002;
    }
}
