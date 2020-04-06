using fNbt;
using System.Text;
using UnityEngine;

public class UnitStats {

    // TODO what's this for?
    public bool dirty;

    private string firstName;
    private string lastName;
    private EnumGender gender;
    private UnitData unitData;
    private float hunger;
    private float sleep;

    public readonly StatisticFloat distanceWalked;
    public readonly StatisticTime timeAlive;
    public readonly StatisticInt unitsKilled;
    public readonly StatisticInt buildingsDestroyed;
    public readonly StatisticInt damageDelt;
    public readonly StatisticInt damageTaken;

    // Builder specific
    public readonly StatisticInt resourcesCollected;
    public readonly StatisticInt buildingsBuilt;
    public readonly StatisticInt repairsDone;

    private UnitStats() {
        this.distanceWalked = new StatisticFloat(this, "Distance Walked", "disWalked");
        this.timeAlive = new StatisticTime(this, "Time Alive", "timeAlive");
        this.unitsKilled = new StatisticInt(this, "Units Killed", "uKills");
        this.buildingsDestroyed = new StatisticInt(this, "Buildings Destroyed", "buildingsDestoryed");
        this.damageDelt = new StatisticInt(this, "Damage Delt", "damageDelt");
        this.damageTaken = new StatisticInt(this, "Damage Taken", "damageTaken");
        this.resourcesCollected = new StatisticInt(this, "Resources Collected", "resCollected");
        this.buildingsBuilt = new StatisticInt(this, "Buildings Built", "buildingsBuilt");
        this.repairsDone = new StatisticInt(this, "Repairs Done", "repairsDone");
    }

    public UnitStats(UnitData unitData) : this() {
        this.unitData = unitData;

        int easterEggRnd = Random.Range(0, 100000); // 100 thousand
        if(easterEggRnd == 1) {
            this.firstName = "Dalton";
            this.lastName = "Didelot";
            this.gender = EnumGender.MALE;
        }
        else if(easterEggRnd == 111599) {
            this.firstName = "PJ";
            this.lastName = "Didelot";
            this.gender = EnumGender.MALE;
        }
        else {
            Names.getRandomName(this.gender, out this.firstName, out this.lastName);
            this.gender = EnumGender.MALE; // Random.Range(0, 1) == 0 ? EnumGender.MALE : EnumGender.FEMALE;
        }

        this.hunger = 1f;
        this.sleep = 1f;
    }

    public UnitStats(NbtCompound tag, UnitData unitData) : this() {
        this.unitData = unitData;

        NbtCompound tag1 = tag.getCompound("stats");

        this.firstName = tag1.getString("firstName");
        this.lastName = tag1.getString("lastName");
        this.gender = tag1.getByte("gender") == 1 ? EnumGender.MALE : EnumGender.FEMALE;

        this.distanceWalked.readFromNbt(tag1);
        this.timeAlive.readFromNbt(tag1);
        this.unitsKilled.readFromNbt(tag1);
        this.buildingsDestroyed.readFromNbt(tag1);
        this.damageDelt.readFromNbt(tag1);
        this.damageTaken.readFromNbt(tag1);
        this.resourcesCollected.readFromNbt(tag1);
        this.buildingsBuilt.readFromNbt(tag1);
        this.repairsDone.readFromNbt(tag1);

        this.hunger = tag1.getFloat("hunger", 1f);
        this.sleep = tag1.getFloat("sleep", 1f);
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

    public float getHunger() {
        return this.hunger;
    }

    public float getSleep() {
        return this.sleep;
    }

    public void setHunger(float hunger) {
        this.hunger = Mathf.Clamp01(hunger);
    }

    public void setSleep(float sleep) {
        this.sleep = Mathf.Clamp01(sleep);
    }

    public void writeToNBT(NbtCompound tag) {
        NbtCompound tag1 = new NbtCompound("stats");

        tag1.setTag("firstName", this.firstName);
        tag1.setTag("lastName", this.lastName);
        tag1.setTag("gender", (byte)(this.gender == EnumGender.MALE ? 1 : 2));

        this.distanceWalked.writeToNbt(tag1);
        this.timeAlive.writeToNbt(tag1);
        this.unitsKilled.writeToNbt(tag1);
        this.buildingsDestroyed.writeToNbt(tag1);
        this.damageDelt.writeToNbt(tag1);
        this.damageTaken.writeToNbt(tag1);
        this.resourcesCollected.writeToNbt(tag1);
        this.buildingsBuilt.writeToNbt(tag1);
        this.repairsDone.writeToNbt(tag1);

        tag1.setTag("hunger", this.hunger);
        tag1.setTag("sleep", this.sleep);

        tag.Add(tag1);
    }

    public string getFormattedStatString(bool isBuilder) {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Health: " + this.unitData.maxHealth);
        sb.AppendLine("Attack: " + this.unitData.attack);
        sb.AppendLine("Defense: " + this.unitData.defense);

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