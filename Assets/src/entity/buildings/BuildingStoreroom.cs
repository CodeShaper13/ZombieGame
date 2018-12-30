using UnityEngine;

public class BuildingStoreroom : BuildingBase {

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
}