using System;
using UnityEngine;

public class HarvestableObject : LivingObject {

    public static readonly Predicate<MapObject> predicateIsHarvestable = (MapObject obj) => { return obj is HarvestableObject; };

    [SerializeField]
    private EnumHarvestableType type;
    private float sizeRadius;

    public override void onStart() {
        base.onStart();

        this.sizeRadius = this.calculateSize();
    }

    private float calculateSize() {
        CapsuleCollider cc = this.GetComponent<CapsuleCollider>();
        if(cc != null) {
            return cc.radius * Mathf.Max(this.transform.localScale.x, this.transform.localScale.z);
        }

        SphereCollider sc = this.GetComponent<SphereCollider>();
        if(sc != null) {
            return sc.radius * Mathf.Max(this.transform.localScale.x, this.transform.localScale.z);
        }

        BoxCollider bc = this.GetComponent<BoxCollider>();
        if(bc != null) {
            return Mathf.Max(bc.size.x * this.transform.localScale.x, bc.size.z * this.transform.localScale.z) / 2;
        }

        MeshCollider mc = this.GetComponent<MeshCollider>();
        if(mc != null) {
            throw new Exception("Meshcollider size is not yet implemented!");
        }

        return 0;
    }

    /// <summary>
    /// Returns true if the object was destroyed.
    /// </summary>
    public virtual bool harvest(UnitBuilder builder) {
        int remainingSpace = Constants.BUILDER_MAX_CARRY - builder.getHeldResources();
        int amountToHarvest = Mathf.Min(Constants.BUILDER_COLLECT_PER_STRIKE, remainingSpace);
        builder.increaseResources(amountToHarvest);

        return this.damage(builder, amountToHarvest);
    }

    public override float getSizeRadius() {
        return this.sizeRadius;
    }

    /// <summary>
    /// Returns the number of resources/health this object has.
    /// </summary>
    private int getTotalResourceYield() {
        switch(this.type) {
            case EnumHarvestableType.TREE:
                return 150;
            case EnumHarvestableType.DEAD_TREE:
                return 100;
            case EnumHarvestableType.TALL_CACTUS:
                return 200;
            case EnumHarvestableType.ROCK:
                return 150;
            case EnumHarvestableType.SKULL:
                return 300;
        }
        return 0;
    }

    public override int getMaxHealth() {
        return this.getTotalResourceYield();
    }

    public override float getHealthBarHeight() {
        switch(this.type) {
            case EnumHarvestableType.TREE:
                return this.transform.localScale.y * 6.8f;
            case EnumHarvestableType.DEAD_TREE:
                return 2.65f;
            case EnumHarvestableType.TALL_CACTUS:
                return 2.75f;
            case EnumHarvestableType.ROCK:
                return 1f;
            case EnumHarvestableType.SKULL:
                return 1f;
        }
        return 0f;
    }
}