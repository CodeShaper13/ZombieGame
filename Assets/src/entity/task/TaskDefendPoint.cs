using UnityEngine;

public class TaskDefendPoint : TaskAttackNearby {

    private Vector3 defendPoint;

    public TaskDefendPoint(UnitFighting unit) : base(unit) {
        defendPoint = this.unit.transform.position;
    }

    public override void drawDebug() {
        base.drawDebug();

        // Draw a line to mark the point and a line to conenct the unit to it's point.
        GLDebug.DrawLine(defendPoint, defendPoint + Vector3.up * 4, Color.black);
        GLDebug.DrawLine(unit.getPos(), defendPoint, Color.gray);
        foreach(Vector3 v in Direction.CARDINAL) {
            GLDebug.DrawLine(defendPoint, defendPoint + (v * Constants.AI_FIGHTING_DEFEND_RANGE), Color.black);
        }
    }

    protected override void preformAttack() {
        if(Util.isAlive(target) && this.withinArea()) {
            func();
        }
        else {
            // We have no target or it is out of range, find a new one.
            target = findTarget();
            if(target == null) {
                // Move back to the defend point.
                moveHelper.setDestination(defendPoint, 0.5f);
            }
        }
    }

    protected override SidedEntity findTarget() {
        return this.findEntityOfType<SidedEntity>(defendPoint, Constants.AI_FIGHTING_DEFEND_RANGE);
    }

    private bool withinArea() {
        return Vector2.Distance(defendPoint, target.getPos()) < Constants.AI_FIGHTING_DEFEND_RANGE;
    }
}
