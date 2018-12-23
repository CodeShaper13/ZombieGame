using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetHandlerServer : NetHandlerBase {

    private Map map;

    public NetHandlerServer(Map map) {
        this.map = map;
    }

    protected override void registerHandlers() {
        this.func01<MessageSpawnEntity>();
        this.func01<MessageRemoveEntity>();
        this.func01<MessageSetObjectPostion>();
        this.func01<MessageRunAction>();
        this.func01<MessageSetUnitDestination>();
    }

    public void spawnEntity(MessageSpawnEntity msg) {
        SpawnInstructions<SidedEntity> instructions = this.map.spawnEntity<SidedEntity>(
            Registry.getObjectFromRegistry(msg.entityId).getPrefab(),
            msg.position,
            msg.rotation);
        Team team = msg.getTeam();
        instructions.getObj().setTeam(msg.getTeam());
        instructions.spawn();
    }

    public void removeEntity(MessageRemoveEntity msg) {
        Map.instance.removeEntity(msg.gameObj.GetComponent<MapObject>());
    }

    public void moveObject(MessageSetObjectPostion msg) {
        msg.target.transform.position = msg.newPosition;
    }

    public void callActionButtonFunction(MessageRunAction msg) {
        ActionButton ab = ActionButton.BUTTON_LIST[msg.buttonID];
        if(msg.childIndex != -1) {
            ab = ((ActionButtonParent)ab).getChildButtons()[msg.childIndex];
        }

        List<SidedEntity> list = new List<SidedEntity>();
        foreach(GameObject obj in msg.targets) {
            list.Add(obj.GetComponent<SidedEntity>());
        }

        ab.callFunction(list);
    }

    public void func02(MessageSetUnitDestination msg) {
        msg.obj.GetComponent<UnitBase>().walkToPoint(msg.destination, 1); // TODO send party size.
    }

    private void func01<T>() where T : AbstractMessage<NetHandlerServer>, new() {
        NetworkServer.RegisterHandler(new T().getID(), delegate (NetworkMessage netMsg) {
            T msg = netMsg.ReadMessage<T>();
            msg.processMessage(this);
        });
    }
}
