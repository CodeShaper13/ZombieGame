public abstract class UnitFighting : UnitBase {

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.idle.getMask() | ActionButton.attackNearby.getMask() | ActionButton.defend.getMask();
    }
}
