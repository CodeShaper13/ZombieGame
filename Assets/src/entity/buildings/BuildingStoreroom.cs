using UnityEngine;

public class BuildingStoreroom : BuildingBase {

    public override float getHealthBarHeight() {
        return 2f;
    }

    public override Vector2 getFootprintSize() {
        return Vector2.one;
    }

    public override BuildingData getData() {
        return Constants.BD_STOREROOM;
    }
}