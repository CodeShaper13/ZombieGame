using fNbt;
using UnityEngine;
using UnityEngine.Networking;

public abstract class BuildingResourceHolder : BuildingBase, IResourceHolder {

    [SerializeField] // Debug
    [SyncVar(hook = "hookOnChangeResources")]
    protected int heldResources;

    private ProgressBar resourceBar;

    public override void onUiInit() {
        base.onUiInit();

        this.resourceBar = ProgressBar.instantiateBar(
            this.gameObject,
            this.getHealthBarHeight() - 0.35f,
            this.getHoldLimit(),
            (curentValue, maxValue) => {
                return Colors.blue;
            });

        this.hookOnChangeResources(this.heldResources);
    }

    public int getHeldResources() {
        return this.heldResources;
    }

    public abstract int getHoldLimit();

    public bool canHoldMore() {
        return this.heldResources < this.getHoldLimit();
    }

    public void setHeldResources(int amount) {
        this.heldResources = amount;
    }

    private void hookOnChangeResources(int resources) {
        this.heldResources = resources;
        if(this.resourceBar != null) {
            this.resourceBar.updateProgressBar(resources);
        }
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.heldResources = tag.getInt("heldResources");
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("heldResources", this.heldResources);
    }
}
