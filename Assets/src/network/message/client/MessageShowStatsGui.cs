using fNbt;
using System;
using UnityEngine.Networking;

public class MessageShowStatsGui : AbstractMessageClient {

    public readonly string nameString;
    public readonly string statsString;

    public MessageShowStatsGui() { }

    public MessageShowStatsGui(UnitBase unit, UnitStats unitStats) {
        this.nameString = unitStats.getName() + "\n" + unit.getData().getUnitTypeName();
        this.statsString = unitStats.getFormattedStatString(unit is UnitBuilder);
    }

    public override short getID() {
        return 2003;
    }

    public override void processMessage(NetHandlerClient handler) {
        handler.showStatsGui(this);
    }
}
