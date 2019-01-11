using UnityEngine;

public class BuildingStoreroom : BuildingResourceHolder {

    public MeshRenderer domeRenderer;

    public override void colorObject() {
        this.domeRenderer.material.color = this.getTeam().getColor();
    }

    public override float getHealthBarHeight() {
        return 2f;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(5, 5);
    }

    public override BuildingData getData() {
        return Constants.BD_STOREROOM;
    }

    public override int getHoldLimit() {
        return Constants.BUILDING_STOREROOM_MAX_HOLD;
    }
}