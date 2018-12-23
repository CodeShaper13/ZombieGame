using UnityEngine;
using UnityEngine.Networking;

public class UnitBuilder : UnitBase, IResourceHolder {

    [SerializeField] // Debug
    [SyncVar(hook = "hookOnChangeResources")]
    private int heldResources;
    private ProgressBar resourceBar;

    public override void OnStartClient() {
        base.OnStartClient();

        this.resourceBar = ProgressBar.instantiateBar(
            this.gameObject,
            base.getHealthBarHeight(),
            this.getHoldLimit(),
            (curentValue, maxValue) => {
                return Colors.blue;
            });
    }

    /// <summary>
    /// Sets the held resources to 0 and returns what
    /// the builder is carrying.
    /// </summary>
    public int deposite() {
        int i = this.heldResources;
        this.heldResources = 0;
        return i;
    }

    public void increaseResources(int amount) {
        this.heldResources += amount;
        //this.getTeam().increaseResources(amount);

        this.unitStats.resourcesCollected.increase(amount);
    }

    public override int getAttackAmount() {
        return 5;
    }

    public override int getMaxHealth() {
        return 50;
    }

    public int getHeldResources() {
        return this.heldResources;
    }

    public int getHoldLimit() {
        return Constants.BUILDER_MAX_CARRY;
    }

    public bool canHoldMore() {
        return this.heldResources < this.getHoldLimit();
    }

    public override EntityBaseStats getData() {
        return Constants.ED_BUILDER;
    }

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.builderBuild.getMask() | ActionButton.harvestResources.getMask() | ActionButton.repair.getMask();
    }

    public override float getHealthBarHeight() {
        return base.getHealthBarHeight() + 0.35f;
    }

    private void hookOnChangeResources(int resources) {
        this.heldResources = resources;
        this.resourceBar.updateProgressBar(resources);
    }
}
