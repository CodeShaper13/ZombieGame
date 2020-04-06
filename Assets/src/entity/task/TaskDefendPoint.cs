using fNbt;
using UnityEngine;

public class TaskDefendPoint : TaskAttackNearby {

    /// <summary>
    /// The point the unit is defending.
    /// </summary>
    private Vector3 pointToDefend;

    public TaskDefendPoint(UnitFighting unit) : base(unit) {
        this.pointToDefend = this.unit.transform.position;
    }

    public override void drawDebug() {
        base.drawDebug();

        // Draw a line to mark the point and a line to conenct the unit to it's point.
        GLDebug.DrawLine(pointToDefend, pointToDefend + Vector3.up * 4, Color.black);
        GLDebug.DrawLine(unit.getPos(), pointToDefend, Color.gray);
        foreach(Direction d in Direction.horizontal) {
            GLDebug.DrawLine(pointToDefend, pointToDefend + (d.vector * Constants.AI_FIGHTING_DEFEND_RANGE), Color.black);
        }
    }

    protected override bool preformAttack() {
        if(Util.isAlive(target) && this.isTargetWithinArea()) {
            this.func();
        } else {
            // We have no target or it is out of range, find a new one.
            this.target = findTarget();
            if(target == null) {
                // Move back to the defend point.
                this.moveHelper.setDestination(pointToDefend, 0.5f);
            }
        }

        return Util.isAlive(this.target);
    }

    protected override SidedEntity findTarget() {
        return this.findEntityOfType<SidedEntity>(pointToDefend, Constants.AI_FIGHTING_DEFEND_RANGE);
    }

    /// <summary>
    /// Returns true if the target is within the defend area.
    /// </summary>
    private bool isTargetWithinArea() {
        return Vector2.Distance(pointToDefend, target.getPos()) < Constants.AI_FIGHTING_DEFEND_RANGE;
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.pointToDefend = tag.getVector3("defendPoint");
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("defendPoint", this.pointToDefend);
    }
}
