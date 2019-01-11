using fNbt;
using UnityEngine;

public class BuildingProducer : BuildingResourceHolder {

    private float time;    

    protected override void preformTask(float deltaTime) {
        if(this.canHoldMore()) {
            this.time -= deltaTime;
            if(this.time < 0) {
                this.time = Constants.BUILDING_PRODUCER_RATE;
                this.heldResources++;
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

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.buildingProducerCollect;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(3, 3);
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.time = tag.getFloat("produceTime");
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("produceTime", this.time);
    }

    public override int getHoldLimit() {
        return Constants.BUILDING_PRODUCER_MAX_HOLD;
    }
}