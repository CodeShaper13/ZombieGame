using fNbt;

public class StatisticFloat : StatisticBase<float> {

    public StatisticFloat(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

    public override void increase(float amount) {
        base.increase(amount);
        this.value += amount;
    }

    public override void readFromNbt(NbtCompound tag) {
        this.value = tag.getFloat(this.saveName);
    }

    public override void writeToNbt(NbtCompound tag) {
        tag.setTag(this.saveName, this.value);
    }
}