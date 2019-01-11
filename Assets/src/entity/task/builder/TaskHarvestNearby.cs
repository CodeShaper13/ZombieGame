using fNbt;
using UnityEngine;

public class TaskHarvestNearby : TaskBase<UnitBuilder>, ICancelableTask {

    private HarvestableObject target;
    private float cooldown;
    private BuildingBase dropoffPoint;

    public TaskHarvestNearby(UnitBuilder unit) : base(unit) { }

    public override void drawDebug() {
        base.drawDebug();

        // Draw lines to the target and drop off point.
        bool flag = this.unit.canHoldMore();
        if(Util.isAlive(this.target)) {
            GLDebug.DrawLine(this.unit.getPos(), this.target.getPos() + Vector3.up, this.func() ? Colors.green : Colors.red);
        }
        if(this.dropoffPoint != null) {
            GLDebug.DrawLine(this.unit.getPos(), this.dropoffPoint.getPos(), flag ? Colors.red : Colors.green);
        }
    }

    private bool func() {
        return this.getDistance(this.target) <= (this.unit.getSizeRadius() + this.target.getSizeRadius() + 0.5f);
    }

    public override bool preform(float deltaTime) {
        this.cooldown -= deltaTime;

        if(this.unit.canHoldMore()) {
            // Find something to harvest.
            if(this.target == null) {
                this.target = (HarvestableObject)this.unit.map.findClosestObject(this.unit.getFootPos(), EntitySelecter.isHarvestable);
            }

            if(this.target == null) {
                return false; // No target to be found, stop executing.
            }
            else {
                this.moveHelper.setDestination(this.target);
            }

            // Harvest from the nearby object if it is in range.
            Debug.Log("func: " + ((this.unit.getSizeRadius() + this.target.getSizeRadius() + 1f)));
            Debug.Log("dis:  " + this.getDistance(this.target));
            if(this.cooldown <= 0 && this.func()) {
                if(this.target.harvest(this.unit)) {
                    // Target was consumed.
                    this.target = null;
                }
                this.cooldown = Constants.BUILDER_STRIKE_RATE;
            }
        }
        else {
            // Full, drop off resources.
            if(this.dropoffPoint == null) {
                // find a drop off point.
                this.dropoffPoint = this.findEntityOfType<BuildingStoreroom>(this.unit.getPos(), -1, false);

                if(this.dropoffPoint == null) {
                    // No drop off point, stop executing, as we can't drop off what we have.
                    return false;
                }
                else {
                    // this.setDestination
                }
            }
        }

        return true;
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.cooldown = tag.getFloat("cooldown");
        this.target = this.unit.map.findMapObjectFromGuid<HarvestableObject>(tag.getGuid("target"));
        this.dropoffPoint = this.unit.map.findMapObjectFromGuid<BuildingBase>(tag.getGuid("dropoffPoint"));
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("cooldown", this.cooldown);
        if(this.target != null) {
            tag.setTag("target", this.target.getGuid());
        }
        if(this.dropoffPoint != null) {
            tag.setTag("dropoffPoint", this.dropoffPoint);
        }
    }
}