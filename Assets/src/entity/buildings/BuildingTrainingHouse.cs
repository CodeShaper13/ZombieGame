using UnityEngine;

public class BuildingTrainingHouse : BuildingQueuedProducerBase {

    public override float getHealthBarHeight() {
        return 3f;
    }

    public override Vector2 getFootprintSize() {
        return Vector2.one;
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