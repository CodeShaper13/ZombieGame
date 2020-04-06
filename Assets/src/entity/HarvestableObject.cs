using System;
using UnityEngine;

public class HarvestableObject : LivingObject {

    /// <summary> The radius of this object, computed at startup and saved. </summary>
    private float sizeRadius;

    public string objectName;
    public int resourceYeild;

    /// <summary>
    /// Returns true if the object was destroyed.
    /// </summary>
    public virtual bool harvest(UnitBuilder builder) {
        int remainingSpace = builder.getHoldLimit() - builder.getHeldResources();
        int amountToHarvest = Mathf.Min(builder.collectPerStrike, remainingSpace);

        this.map.transferResources(builder.getTeam(), amountToHarvest); // Extra is discarded.

        return this.damage(builder, amountToHarvest);
    }

    public override float getSizeRadius() {
        return 0.5f;
    }

    /// <summary>
    /// Returns the number of resources/health this object has.
    /// </summary>
    protected int getTotalResourceYield() {
        return this.resourceYeild;
    }

    public override int getMaxHealth() {
        return this.getTotalResourceYield();
    }

    public override bool shouldHealthbarBeShown() {
        return this.getHealth() < this.getMaxHealth();
    }

    public override float getHealthBarHeight() {
        return 0.75f;
    }

    public override string getDisplayName() {
        return this.objectName;
    }
}