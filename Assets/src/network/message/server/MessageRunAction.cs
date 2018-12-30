using System.Collections.Generic;
using UnityEngine;

public class MessageRunAction : AbstractMessage<NetHandlerServer> {

    public readonly int buttonID;
    public readonly int childIndex;
    public readonly GameObject[] targets;

    public MessageRunAction() { }

    public MessageRunAction(int buttonID, List<SidedEntity> objs) : this(buttonID, -1, objs) { }

    public MessageRunAction(int buttonID, int childIndex, List<SidedEntity> objs) {
        this.buttonID = buttonID;
        this.childIndex = childIndex;
        this.targets = new GameObject[objs.Count];
        for(int i = 0; i < objs.Count; i++) {
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
