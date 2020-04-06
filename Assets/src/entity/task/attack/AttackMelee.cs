using UnityEngine;

public class AttackMelee : AttackBase {

    public AttackMelee(UnitBase unit) : base(unit) { }

    /// <summary>
    /// Returns true if the target is in range.
    /// </summary>
    public override bool inRangeToAttack(LivingObject target) {
        return Vector3.Distance(this.unit.transform.position, target.transform.position) <= 1.25f;
    }

    protected override float getAttackRate() {
        return Constants.AI_MELEE_ATTACK_RATE;
    }

    protected override void preformAttack(LivingObject target) {
        this.unit.damageTarget(target);
    }
}
