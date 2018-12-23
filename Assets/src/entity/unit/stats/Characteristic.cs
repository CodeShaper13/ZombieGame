public class Characteristic {

    private static readonly float FACTOR = 0.10f;

    public static readonly Characteristic[] ALL = new Characteristic[16];

    /// <summary> HP +1   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic a = new Characteristic(0, "", 1, 0, 0, 0);
    /// <summary> HP  0   Speed +1   Attack  0   Defense  0 </summary>
    public static readonly Characteristic b = new Characteristic(1, "", 0, 1, 0, 0);
    /// <summary> HP  0   Speed  0   Attack +1   Defense  0 </summary>
    public static readonly Characteristic c = new Characteristic(2, "", 0, 0, 1, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense +1 </summary>
    public static readonly Characteristic d = new Characteristic(3, "", 0, 0, 0, 1);

    /// <summary> HP +2   Speed  0   Attack -1   Defense  0 </summary>
    public static readonly Characteristic e = new Characteristic(4, "", 2, 0, -1, 0);
    /// <summary> HP  0   Speed +2   Attack  0   Defense -1 </summary>
    public static readonly Characteristic f = new Characteristic(5, "", 0, 2, 0, -1);
    /// <summary> HP -1   Speed  0   Attack +2   Defense  0 </summary>
    public static readonly Characteristic g = new Characteristic(6, "", -1, 0, 2, 0);
    /// <summary> HP  0   Speed -1   Attack  0   Defense +2 </summary>
    public static readonly Characteristic h = new Characteristic(7, "", 0, -1, 0, 2);

    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic i = new Characteristic(8, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic j = new Characteristic(9, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic k = new Characteristic(10, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic l = new Characteristic(11, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic m = new Characteristic(12, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic n = new Characteristic(13, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic o = new Characteristic(14, "", 0, 0, 0, 0);
    /// <summary> HP  0   Speed  0   Attack  0   Defense  0 </summary>
    public static readonly Characteristic p = new Characteristic(15, "", 0, 0, 0, 0);

    private readonly int id;
    private readonly string s;
    private readonly int hpMod;
    private readonly int speedMod;
    private readonly int attackMod;
    private readonly int defenseMod;

    public Characteristic(int id, string s, int hpMod, int speedMod, int attackMod, int defenseMod) {
        this.id = id;
        this.s = s;
        this.hpMod = hpMod;
        this.speedMod = speedMod;
        this.attackMod = attackMod;
        this.defenseMod = defenseMod;

        Characteristic.ALL[this.id] = this;
    }

    public string getDescription() {
        return this.s;
    }

    /// <summary>
    /// Returns the id of the characteristic.
    /// </summary>
    public int getId() {
        return this.id;
    }

    public int getHealth(int baseHealth) {
        return (int)this.func(baseHealth, this.hpMod);
    }

    public float getSpeed(float baseSpeed) {
        return this.func(baseSpeed, this.speedMod);
    }

    public int getAttack(int baseAttack) {
        return (int)this.func(baseAttack, this.attackMod);
    }

    public int getDefense(int baseDefense) {
        return (int)this.func(baseDefense, this.defenseMod);
    }

    private float func(float baseStat, int mod) {
        return (mod * FACTOR * baseStat) + baseStat;
    }
}