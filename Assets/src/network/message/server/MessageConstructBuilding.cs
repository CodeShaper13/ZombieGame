using System;
using UnityEngine;
using UnityEngine.Networking;

public class MessageConstructBuilding : AbstractMessage<NetHandlerServer> {

    public int entityId;
    public Vector3 position;
    public int teamId;
    public Guid builderGuid;

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

    public override void processMessage(NetHandlerServer handler) {
        handler.constructBuilding(this);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(this.entityId);
        writer.Write(this.position);
        writer.Write(this.teamId);
        writer.Write(this.builderGuid.ToByteArray(), 16);
    }

    public override void Deserialize(NetworkReader reader) {
        this.entityId = reader.ReadInt32();
        this.position = reader.ReadVector3();
        this.teamId = reader.ReadInt32();
        this.builderGuid = new Guid(reader.ReadBytes(16));
    }
}
