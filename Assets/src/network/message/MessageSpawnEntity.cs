using UnityEngine;

public class MessageSpawnEntity : AbstractMessage {

    public int entityId;
    public Vector3 position;
    public Quaternion rotation;
    public int teamId;

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

    public override void processMessage(MessageProcesser ms) {
        ms.spawnEntity(this);
    }

    public override short getID() {
        return 1003;
    }
}
