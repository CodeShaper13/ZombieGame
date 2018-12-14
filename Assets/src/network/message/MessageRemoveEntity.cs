using UnityEngine;

public class MessageRemoveEntity : AbstractMessage {

    public GameObject gameObj;

    public MessageRemoveEntity() { }

    public MessageRemoveEntity(MapObject mapObj) {
        this.gameObj = mapObj.gameObject;
    }

    public override void processMessage(MessageProcesser ms) {
        ms.removeEntity(this);
    }

    public override short getID() {
        return 1004;
    }
}
