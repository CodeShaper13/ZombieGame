using System;
using UnityEngine.Networking;

/// <summary>
/// A message that requests the server to send a message containting the stats of a unit.
/// </summary>
public class MessageRequestStats : AbstractMessageServer {

    public Guid unitGuid;

    public MessageRequestStats() { }

    public MessageRequestStats(UnitBase unit) {
        this.unitGuid = unit.getGuid();
    }

    public override short getID() {
        return 1007;
    }

    public override void processMessage(ConnectedPlayer sender, NetHandlerServer handler) {
        handler.requestStats(sender, this);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(this.unitGuid.ToByteArray(), 16);
    }

    public override void Deserialize(NetworkReader reader) {
        this.unitGuid = new Guid(reader.ReadBytes(16));
    }
}
