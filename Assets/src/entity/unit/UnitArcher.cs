public class UnitArcher : UnitFighting {

    public override AttackBase createAttackMethod() {
        return new AttackArrow(this);
    }
}
