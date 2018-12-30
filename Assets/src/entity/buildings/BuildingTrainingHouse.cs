using UnityEngine;

public class BuildingTrainingHouse : BuildingQueuedProducerBase {

    public override void colorObject() {
        this.GetComponent<MeshRenderer>().material.color = this.getTeam().getColor();
    }

    public override float getHealthBarHeight() {
        return 3f;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(3, 3);
    }

    public override BuildingData getData() {
        return Constants.BD_TRAINING_HOUSE;
    }

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.train.getMask();
    }

    public override int getQueueSize() {
        return Constants.BUILDING_TRAINING_HOUSE_QUEUE_SIZE;
    }
}