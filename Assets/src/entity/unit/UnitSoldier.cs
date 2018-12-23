public class UnitSoldier : UnitFighting {

    public override int getAttackAmount() {
        return 10;
    }

    public override int getMaxHealth() {
        return 50;
    }

    public override EntityBaseStats getData() {
        return Constants.ED_SOLDIER;
    }
}
