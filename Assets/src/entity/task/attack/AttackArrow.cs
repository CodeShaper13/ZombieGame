using UnityEngine;

public class AttackArrow : AttackBase {

    public AttackArrow(UnitBase unit) : base(unit) { }

    public override bool inRangeToAttack(LivingObject target) {
        return Vector3.Distance(this.unit.getPos(), target.getPos()) <= Constants.AI_ARCHER_SHOOT_RANGE;
    }

    protected override void preformAttack(LivingObject target) {
        this.unit.map.fireProjectile(
            this.unit.transform.position + new Vector3(0, 1f, 0),
            this.unit,
            this.unit.getAttackAmount(),
            target);
    }
}
