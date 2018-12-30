public abstract class UnitFighting : UnitBase {

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.unitIdle.getMask() | ActionButton.unitAttackNearby.getMask() | ActionButton.unitDefend.getMask();
    }
}
