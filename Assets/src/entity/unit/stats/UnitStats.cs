using System.Text;
using UnityEngine;

public class UnitStats {

    // TODO what's this for?
    public bool dirty;

    private string firstName;
    private string lastName;
    private EnumGender gender;
    private Characteristic characteristic;
    private EntityBaseStats baseStats;

    public readonly StatFloat distanceWalked;
    public readonly StatTime timeAlive;
    public readonly StatInt unitsKilled;
    public readonly StatInt buildingsDestroyed;
    public readonly StatInt damageDelt;
    public readonly StatInt damageTaken;

    // Builder specific
    public readonly StatInt resourcesCollected;
    public readonly StatInt buildingsBuilt;
    public readonly StatInt repairsDone;

    private UnitStats() {
        this.distanceWalked = new StatFloat(this, "Distance Walked", "disWalked");
        this.timeAlive = new StatTime(this, "Time Alive", "timeAlive");
        this.unitsKilled = new StatInt(this, "Units Killed", "uKills");
        this.buildingsDestroyed = new StatInt(this, "Buildings Destroyed", "buildingsDestoryed");
        this.damageDelt = new StatInt(this, "Damage Delt", "damageDelt");
        this.damageTaken = new StatInt(this, "Damage Taken", "damageTaken");
        this.resourcesCollected = new StatInt(this, "Resources Collected", "resCollected");
        this.buildingsBuilt = new StatInt(this, "Buildings Built", "buildingsBuilt");
        this.repairsDone = new StatInt(this, "Repairs Done", "repairsDone");
    }

    public UnitStats(EntityBaseStats baseStats) : this() {
        this.baseStats = baseStats;

        int easterEggRnd = Random.Range(0, 100000);
        if(easterEggRnd == 111599) {
            this.firstName = "PJ";
            this.lastName = "Didelot";
            this.gender = EnumGender.MALE;
            this.characteristic = Characteristic.a; // TODO Set to the right thing.
        }
        else {
            Names.getRandomName(this.gender, out this.firstName, out this.lastName);
            this.gender = Random.Range(0, 1) == 0 ? EnumGender.MALE : EnumGender.FEMALE;
            this.characteristic = Characteristic.ALL[Random.Range(0, Characteristic.ALL.Length)];
        }
    }

    /// <summary>
    /// Returns the full name, first and last, of the unit.
    /// </summary>
    public string getName() {
        return this.firstName + " " + this.lastName;
    }

    public EnumGender getGender() {
        return this.gender;
    }

    public int getMaxHealth() {
        return this.characteristic.getHealth(this.baseStats.baseHealth);
    }

    public float getSpeed() {
        return this.characteristic.getSpeed(this.baseStats.baseSpeed);
    }

    public int getAttack() {
        return this.characteristic.getAttack(this.baseStats.baseAttack);
    }

    public int getDefense() {
        return this.characteristic.getDefense(this.baseStats.baseDefense);
    }

    public string getFormattedStatString(bool isBuilder) {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Health: " + this.getMaxHealth());
        sb.AppendLine("Speed: " + this.getSpeed());
        sb.AppendLine("Attack: " + this.getAttack());
        sb.AppendLine("Defense: " + this.getDefense());

        sb.AppendLine();

        //sb.AppendLine((System.Math.Truncate(this.distanceWalked.get() * 100) / 100) + " km");
        float f = this.distanceWalked.get();
        bool isMeters = f < 1000;
        sb.AppendLine(this.distanceWalked.getDisplayName() + ": " + (isMeters ? f : f / 1000) + (isMeters ? "m" : "km"));
        sb.AppendLine(this.timeAlive.ToString());
        sb.AppendLine(this.unitsKilled.ToString());
        sb.AppendLine(this.buildingsDestroyed.ToString());
        sb.AppendLine(this.damageDelt.ToString());
        sb.AppendLine(this.damageTaken.ToString());

        if(isBuilder) {
            sb.AppendLine(this.resourcesCollected.ToString());
            sb.AppendLine(this.buildingsBuilt.ToString());
            sb.AppendLine(this.repairsDone.ToString());
        }
        return sb.ToString();
    }
}
