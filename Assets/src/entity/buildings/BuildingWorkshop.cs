﻿using UnityEngine;

public class BuildingWorkshop : BuildingQueuedProducerBase {

    public override void colorObject() {
        this.transform.GetComponent<MeshRenderer>().material.color = this.getTeam().getColor();
    }

    public override float getHealthBarHeight() {
        return 2f;
    }

    public override BuildingData getData() {
        return Constants.BD_WORKSHOP;
    }

    public override Vector2 getFootprintSize() {
        return Vector2.one;
    }

    public override int getButtonMask() {
        return base.getButtonMask();
    }

    public override int getQueueSize() {
        return Constants.BUILDING_WORKSHOP_QUEUE_SIZE;
    }
}
