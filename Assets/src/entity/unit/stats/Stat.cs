using System;

public abstract class Stat<T> {

    protected string displayName;
    protected string saveName;
    protected T value;
    private UnitStats unitStats;

    public Stat(UnitStats unitStats, string displayName, string saveName) {
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

    public string getDisplayName() {
        return this.displayName;
    }

    public override string ToString() {
        return this.displayName + ": " + this.value.ToString();
    }
}

public class StatInt : Stat<int> {

    public StatInt(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

    public override void increase(int amount = 1) {
        base.increase(amount);
        this.value += amount;
    }
}

public class StatFloat : Stat<float> {

    public StatFloat(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

    public override void increase(float amount) {
        base.increase(amount);
        this.value += amount;
    }
}

public class StatTime : StatFloat {

    public StatTime(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

    public override string ToString() {
        TimeSpan time = TimeSpan.FromSeconds(this.value);
        string s = time.ToString();
        if(s.Contains(".")) {
            s = s.Substring(0, s.LastIndexOf("."));
        }
        return this.displayName + ": " + s;
    }
}