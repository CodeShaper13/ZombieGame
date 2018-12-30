using fNbt;
using UnityEngine;
using UnityEngine.Networking;

public class BuildingProducer : BuildingBase, IResourceHolder {

    private float time;
    [SerializeField] // Debug
    [SyncVar(hook = "hookOnChangeResources")]
    private int heldResources;

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
    }

    protected override void preformTask(float deltaTime) {
        if(this.heldResources < Constants.BUILDING_PRODUCER_MAX_HOLD) {
            this.time += (deltaTime * Constants.BUILDING_PRODUCER_RATE);

            if(this.time > 0) {
                this.time = 0;
                this.heldResources += 1;
            }
        }
    }

    public override void colorObject() {
        this.transform.GetComponent<MeshRenderer>().material.color = this.getTeam().getColor();
    }

    public override float getHealthBarHeight() {
        return 3f;
    }

    public override BuildingData getData() {
        return Constants.BD_PRODUCER;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(3, 3);
    }

    public override int getButtonMask() {
        return base.getButtonMask();
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.time = tag.getFloat("produceTime");
        this.heldResources = tag.getInt("heldResources");
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("produceTime", this.time);
        tag.setTag("heldResources", this.heldResources);
    }

    public int getHeldResources() {
        return this.heldResources;
    }

    public int getHoldLimit() {
        return Constants.BUILDING_PRODUCER_MAX_HOLD;
    }

    public bool canHoldMore() {
        return this.heldResources < this.getHoldLimit();
    }

    private void hookOnChangeResources(int resources) {
        this.heldResources = resources;
        this.resourceBar.updateProgressBar(resources);
    }
}