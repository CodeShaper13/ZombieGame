using System;
using UnityEngine;

public abstract class HarvestableObject : LivingObject {

    /// <summary> The radius of this object, computed at startup and saved. </summary>
    [SerializeField]
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
            throw new Exception("Mesh Collider size is not yet implemented!");
        }

        return 0;
    }

    /// <summary>
    /// Returns true if the object was destroyed.
    /// </summary>
    public virtual bool harvest(UnitBuilder builder) {
        int remainingSpace = Constants.BUILDER_MAX_CARRY - builder.getHeldResources();
        int amountToHarvest = Mathf.Min(Constants.BUILDER_COLLECT_PER_STRIKE, remainingSpace);
        //builder.increaseResources(amountToHarvest);
        this.map.increaceResources(builder.getTeam(), amountToHarvest);

        return this.damage(builder, amountToHarvest);
    }

    public override float getSizeRadius() {
        return this.sizeRadius;
    }

    /// <summary>
    /// Returns the number of resources/health this object has.
    /// </summary>
    protected abstract int getTotalResourceYield();

    public override int getMaxHealth() {
        return this.getTotalResourceYield();
    }

    public override bool shouldHealthbarBeShown() {
        return this.getHealth() < this.getMaxHealth();
    }
}