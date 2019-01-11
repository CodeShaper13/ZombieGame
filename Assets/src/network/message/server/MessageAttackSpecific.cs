using System;
using UnityEngine.Networking;

public class MessageAttackSpecific : AbstractMessageServer {

    public Guid attackerGuid;
    public Guid targetGuid;

    public MessageAttackSpecific() { }

    public MessageAttackSpecific(UnitBase attacker, LivingObject target) {
        this.attackerGuid = attacker.getGuid();
        this.targetGuid = target.getGuid();
    }

    public override short getID() {
        return 1008;
    }

    public override void processMessage(ConnectedPlayer sender, NetHandlerServer handler) {
        handler.specificAttack(this);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(this.attackerGuid);
        writer.Write(this.targetGuid);
    }

    public override void Deserialize(NetworkReader reader) {
        this.attackerGuid = reader.ReadGuid();
        this.targetGuid = reader.ReadGuid();
    }
}
