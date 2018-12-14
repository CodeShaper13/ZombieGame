using UnityEngine.Networking;

public class MessageProcesser {

    private Map map;

    public MessageProcesser(Map map) {
        this.map = map;
    }

    public void registerHandlers() {
        this.func01<MessageSpawnEntity>();
        this.func01<MessageRemoveEntity>();
    }

    public void spawnEntity(MessageSpawnEntity msg) {
        SidedEntity entity = (SidedEntity)Map.instance.spawnEntity(
            Registry.getObjectFromRegistry(msg.entityId).getPrefab(),
            msg.position,
            msg.rotation);
        Team team = msg.getTeam();
        entity.setTeam(msg.getTeam());
        entity.RpcSetTeam(team.getTeamId());
    }

    public void removeEntity(MessageRemoveEntity msg) {
        Map.instance.removeEntity(msg.gameObj.GetComponent<MapObject>());
    }

    private void func01<T>() where T : AbstractMessage, new() {
        NetworkServer.RegisterHandler(new T().getID(), delegate (NetworkMessage netMsg) {
            T msg = netMsg.ReadMessage<T>();
            msg.processMessage(this);
        });
    }
}
