using UnityEngine;

public class MessageRunAction : AbstractMessage<NetHandlerServer> {

    public readonly int buttonID;
    public readonly int childIndex;
    public readonly GameObject[] targets;

    public MessageRunAction() { }

    public MessageRunAction(int buttonID, params SidedEntity[] objs) : this(buttonID, -1, objs) { }

    public MessageRunAction(int buttonID, int childIndex, params SidedEntity[] objs) {
        this.buttonID = buttonID;
        this.childIndex = childIndex;
        this.targets = new GameObject[objs.Length];
        for(int i = 0; i < objs.Length; i++) {
            this.targets[i] = objs[i].gameObject;
        }
    }

    public override short getID() {
        return 1004;
    }

    public override void processMessage(NetHandlerServer handler) {
        handler.callActionButtonFunction(this);
    }
}
