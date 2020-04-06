using fNbt;
using UnityEngine;

public class TaskHarvestNearby : TaskBase<UnitBuilder>, ICancelableTask {

    private HarvestableObject target;
    private float harvestCooldown;

    public TaskHarvestNearby(UnitBuilder unit, HarvestableObject target) : base(unit) {
        this.target = target;
    }

    public override void drawDebug() {
        base.drawDebug();

        // Draw lines to the target and drop off point.
        bool flag = this.unit.canHoldMore();
        if(Util.isAlive(this.target)) {
            GLDebug.DrawLine(this.unit.getPos(), this.target.getPos() + Vector3.up, this.closeEnough() ? Colors.green : Colors.red);
        }
    }

    /// <summary>
    /// Checks the the unit is close enough to harvest the target.
    /// </summary>
    private bool closeEnough() {
        return Vector3.Distance(this.unit.transform.position, this.target.transform.position) <= 1.1f;
    }

    public override bool preform(float deltaTime) {
        this.harvestCooldown -= deltaTime;

        // Find something to harvest.
        //if(!Util.isAlive(this.target)) {
        //    this.target = (HarvestableObject)this.unit.map.findClosestObject(this.unit.getFootPos(), EntityPredicate.isHarvestable);
        //}

        if(!Util.isAlive(this.target)) {
            return false; // No target to be found, stop executing.
        }
        else {
            this.moveHelper.setDestination(this.target);
        }

        if(this.harvestCooldown <= 0 && this.closeEnough()) {
            if(this.target.harvest(this.unit)) {
                // Target was consumed.
                this.target = null;
                return false;
            }
            this.harvestCooldown = this.unit.strikeSpeed;
        }
        return true;
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.harvestCooldown = tag.getFloat("cooldown");
        this.target = this.unit.map.findMapObjectFromGuid<HarvestableObject>(tag.getGuid("target"));
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("cooldown", this.harvestCooldown);
        if(this.target != null) {
            tag.setTag("target", this.target.getGuid());
        }
    }
}