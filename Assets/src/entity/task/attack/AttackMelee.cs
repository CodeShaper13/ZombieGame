using UnityEngine;

public class AttackMelee : AttackBase {

    public AttackMelee(UnitBase unit) : base(unit) { }

    /// <summary>
    /// Returns true if the target is in range.
    /// </summary>
    public override bool inRangeToAttack(LivingObject target) {
        float maxDistance = unit.getSizeRadius() + target.getSizeRadius() + 0.5f;
        return Vector3.Distance(unit.getPos(), target.transform.position) <= maxDistance;
    }

    protected override void preformAttack(LivingObject target) {
        unit.damageTarget(target);
    }
}
