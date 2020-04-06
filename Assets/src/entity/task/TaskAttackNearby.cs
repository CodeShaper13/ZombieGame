using fNbt;
using UnityEngine;

public class TaskAttackNearby : TaskBase<UnitBase>, ICancelableTask {

    protected LivingObject target;
    protected bool ignoreDistances;

    public TaskAttackNearby(UnitBase unit, LivingObject attackTarget = null) : base(unit) {
        if(attackTarget != null) {
            this.target = attackTarget;
            this.ignoreDistances = true;
        }
        else {
            this.target = this.findTarget();
        }
    }

    public override bool preform(float deltaTime) {
        return this.preformAttack();
    }

    public override void drawDebug() {
        if(Util.isAlive(this.target)) {
            Color c = this.unit.attack.inRangeToAttack(this.target) ? Color.green : Color.red;
            GLDebug.DrawLine(this.unit.getPos(), this.target.getPos(), c);
        }
    }

    protected virtual bool preformAttack() {
        if(Util.isAlive(this.target) && (Vector3.Distance(this.unit.getPos(), this.target.getPos()) <= Constants.AI_FIGHTING_FIND_RANGE || this.ignoreDistances)) {
            // There is a target and it is close enough to "see".
            this.func();
        }
        else {
            // Target is dead or out of range, find a new one.
            this.target = this.findTarget();
        }

        return Util.isAlive(this.target);
    }

    /// <summary>
    /// Finds a new target and returns it, or null is returned if no target can be found.
    /// </summary>
    protected virtual SidedEntity findTarget() {
        return this.findEntityOfType<SidedEntity>(Constants.AI_FIGHTING_FIND_RANGE);
    }

    /// <summary>
    /// Attacks the target if they are in range, or continues to move if they are not in range.
    /// </summary>
    protected void func() {
        if(this.unit.attack.inRangeToAttack(this.target)) {
            this.target = this.unit.attack.attack(this.target);
            this.moveHelper.stop();
        } else {
            this.moveHelper.setDestination(this.target);
        }
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.target = this.unit.map.findMapObjectFromGuid<LivingObject>(tag.getGuid("target"));
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        if(this.target != null) {
            tag.setTag("target", this.target.getGuid());
        }
    }
}
