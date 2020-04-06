using System;
using UnityEngine;
using UnityEngine.Networking;

public class MessageConstructBuilding : AbstractMessageServer {

    public int entityId;
    public Vector3 position;
    public int teamId;
    public Guid builderGuid;

    // Only used when building bridges
    public Vector3 bridgeStart;
    public Vector3 bridgeEnd;

    public MessageConstructBuilding() { }

    public MessageConstructBuilding(Team team, RegisteredObject obj, Vector3 position, UnitBuilder builder) {
        this.teamId = team.getId();
        this.entityId = obj.getId();
        this.position = position;
        this.builderGuid = builder.getGuid();
    }

    public override short getID() {
        return 1006;
    }

    public override void processMessage(ConnectedPlayer sender, NetHandlerServer handler) {
        handler.constructBuilding(this);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(this.entityId);
        writer.Write(this.position);
        writer.Write(this.teamId);
        writer.Write(this.builderGuid);
    }

    public override void Deserialize(NetworkReader reader) {
        this.entityId = reader.ReadInt32();
        this.position = reader.ReadVector3();
        this.teamId = reader.ReadInt32();
        this.builderGuid = reader.ReadGuid();
    }
}
