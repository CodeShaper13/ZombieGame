public class UnitArcher : UnitFighting {

    public override int getAttackAmount() {
        return 8;
    }

    public override int getMaxHealth() {
        return 40;
    }

    public override AttackBase createAttackMethod() {
        return new AttackArrow(this);
    }

    public override EntityBaseStats getData() {
        return Constants.ED_ARCHER;
    }
}
