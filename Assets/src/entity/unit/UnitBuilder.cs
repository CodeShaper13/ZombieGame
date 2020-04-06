using UnityEngine;
using UnityEngine.Networking;

public class UnitBuilder : UnitBase, IResourceHolder {

    [SerializeField]
    private int maxResourceCarry = 1;
    [SerializeField]
    public int collectPerStrike = 1;
    [SerializeField]
    public int strikeSpeed = 1;
    [SerializeField]
    public int buildSpeed = 1;

    [SyncVar(hook = "hookOnChangeResources")]
    private int heldResources;
    private ProgressBar resourceBar;

    public override void onUiInit() {
        base.onUiInit();

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

        this.unitStats.resourcesCollected.increase(amount);
    }

    public int getHeldResources() {
        return this.heldResources;
    }

    public int getHoldLimit() {
        return this.maxResourceCarry;
    }

    public bool canHoldMore() {
        return this.heldResources < this.getHoldLimit();
    }

    public void setHeldResources(int amount) {
        this.heldResources = amount;
    }

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.builderBuild | ActionButton.builderHarvestResources | ActionButton.builderRepair;
    }

    public override float getHealthBarHeight() {
        return base.getHealthBarHeight() + 0.35f;
    }

    private void hookOnChangeResources(int resources) {
        this.heldResources = resources;
        this.resourceBar.updateProgressBar(resources);
    }
}
