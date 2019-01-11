public abstract class UnitFighting : UnitBase {

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.unitIdle | ActionButton.unitAttackNearby | ActionButton.unitDefend;
    }
}
