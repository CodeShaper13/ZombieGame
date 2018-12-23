using UnityEngine;

public class MessageSpawnEntity : AbstractMessage<NetHandlerServer> {

    public readonly int entityId;
    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly int teamId;

    public MessageSpawnEntity() { }

    public MessageSpawnEntity(RegisteredObject registeredObject, Vector3 pos, Vector3 rotation, Team team) {
        this.entityId = registeredObject.getId();
        this.position = pos;
        this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        this.teamId = team.getTeamId();
    }

    public Team getTeam() {
        return Team.getTeamFromId(this.teamId);
    }

    public override void processMessage(NetHandlerServer handler) {
        handler.spawnEntity(this);
    }

    public override short getID() {
        return 1001;
    }
}
