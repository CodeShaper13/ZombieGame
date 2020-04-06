using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetHandlerServer : NetHandlerBase {

    private MapBase map;

    public NetHandlerServer(MapBase map) {
        this.map = map;
    }

    protected override void registerHandlers() {
        this.registerMsg<MessageSpawnEntity>();
        this.registerMsg<MessageRemoveEntity>();
        this.registerMsg<MessageSetObjectPostion>();
        this.registerMsg<MessageRunAction>();
        this.registerMsg<MessageSetUnitDestination>();
        this.registerMsg<MessageConstructBuilding>();
        this.registerMsg<MessageRequestStats>();
        this.registerMsg<MessageAttackSpecific>();
    }

    public void spawnEntity(MessageSpawnEntity msg) {
        SpawnInstructions<SidedEntity> instructions = this.map.spawnEntity<SidedEntity>(
            Registry.getObjectFromRegistry(msg.entityId),
            msg.position,
            msg.rotation);
        instructions.getObj().setTeam(msg.getTeam());
        instructions.spawn();
    }

    public void removeEntity(MessageRemoveEntity msg) {
        MapBase.instance.removeMapObject(msg.gameObj.GetComponent<MapObject>());
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
        msg.obj.GetComponent<UnitBase>().walkToPoint(msg.destination, msg.partySize);
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
            UnitBuilder builder = map.findMapObjectFromGuid<UnitBuilder>(msg.builderGuid);
            builder.setTask(new TaskConstructBuilding(builder, newBuilding));
            builder.unitStats.buildingsBuilt.increase();
        }

        if(newBuilding is BuildingBridge) {

        }

        instructions.spawn();
    }

    public void requestStats(ConnectedPlayer sender, MessageRequestStats msg) {
        UnitBase unit = this.map.findMapObjectFromGuid<UnitBase>(msg.unitGuid);
        if(unit != null) {
            sender.sendMessage(new MessageShowStatsGui(unit, unit.unitStats));
        }
    }

    public void specificAttack(MessageAttackSpecific msg) {
        UnitBase unit = this.map.findMapObjectFromGuid<UnitBase>(msg.attackerGuid);
        LivingObject target = this.map.findMapObjectFromGuid<LivingObject>(msg.targetGuid);
        if(unit != null && target != null) {
            unit.setTask(new TaskAttackNearby(unit, target));
        }
    }

    private void registerMsg<T>() where T : AbstractMessageServer, new() {
        NetworkServer.RegisterHandler(new T().getID(), delegate (NetworkMessage netMsg) {
            T msg = netMsg.ReadMessage<T>();
            msg.processMessage(this.map.connectedPlayerFromNetworkConnection(netMsg.conn), this);
        });
    }
}
