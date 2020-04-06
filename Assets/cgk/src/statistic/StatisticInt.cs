using fNbt;

public class StatisticInt : StatisticBase<int> {

    public StatisticInt(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

    public override void increase(int amount = 1) {
        base.increase(amount);
        this.value += amount;
    }

    public override void readFromNbt(NbtCompound tag) {
        this.value = tag.getInt(this.saveName);
    }

    public override void writeToNbt(NbtCompound tag) {
        tag.setTag(this.saveName, this.value);
    }
}