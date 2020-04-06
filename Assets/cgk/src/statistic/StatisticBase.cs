using fNbt;

public abstract class StatisticBase<T> {

    protected string displayName;
    protected string saveName;
    protected T value;
    private UnitStats unitStats;

    public StatisticBase(UnitStats unitStats, string displayName, string saveName) {
        this.unitStats = unitStats;
        this.displayName = displayName;
        this.saveName = "stat." + saveName;
    }

    public T get() {
        return this.value;
    }

    public virtual void increase(T amount) {
        this.unitStats.dirty = true;
    }

    public abstract void readFromNbt(NbtCompound tag);

    public abstract void writeToNbt(NbtCompound tag);

    public string getDisplayName() {
        return this.displayName;
    }

    public override string ToString() {
        return this.displayName + ": " + this.value.ToString();
    }
}