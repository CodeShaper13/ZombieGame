using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetHandlerServer : NetHandlerBase {

    private Map map;

    public NetHandlerServer(Map map) {
        this.map = map;
    }

    protected override void registerHandlers() {
        this.registerMsg<MessageSpawnEntity>();
        this.registerMsg<MessageRemoveEntity>();
        this.registerMsg<MessageSetObjectPostion>();
        this.registerMsg<MessageRunAction>();
        this.registerMsg<MessageSetUnitDestination>();
        this.registerMsg<MessageConstructBuilding>();
    }

    public void spawnEntity(MessageSpawnEntity msg) {
        SpawnInstructions<SidedEntity> instructions = this.map.spawnEntity<SidedEntity>(
            Registry.getObjectFromRegistry(msg.entityId),
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

    public void constructBuilding(MessageConstructBuilding msg) {
        // Create the new building GameObject and set it's team.
        SpawnInstructions<BuildingBase> instructions = this.map.spawnEntity<BuildingBase>(
            Registry.getObjectFromRegistry(msg.entityId),
            new Vector3(msg.position.x, 0, msg.position.z),
            Quaternion.identity);
        BuildingBase newBuilding = instructions.getObj();
        Team team = Team.getTeamFromId(msg.teamId);
        newBuilding.setTeam(team);

        // Remove resources from the builder's team.
        this.map.reduceResources(team, newBuilding.getData().getCost());

        // Set the buildings health and send the builder to build it.
        BuildingData bd = newBuilding.getData();
        if(bd.isInstantBuild()) {
            newBuilding.setHealth(bd.getMaxHealth());
        }
        else {
            newBuilding.setHealth(1);
            UnitBuilder builder = (UnitBuilder)map.findMapObjectFromGuid(msg.builderGuid);
            builder.setTask(new TaskConstructBuilding(builder, newBuilding));
        }

        instructions.spawn();
    }

    private void registerMsg<T>() where T : AbstractMessage<NetHandlerServer>, new() {
        NetworkServer.RegisterHandler(new T().getID(), delegate (NetworkMessage netMsg) {
            T msg = netMsg.ReadMessage<T>();
            msg.processMessage(this);
        });
    }
}
