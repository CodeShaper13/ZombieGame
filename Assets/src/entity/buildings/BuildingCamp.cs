using UnityEngine;

public class BuildingCamp : BuildingBase {

    public override float getHealthBarHeight() {
        return 2f;
    }

    public override BuildingData getData() {
        return Constants.BD_CAMP;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(5, 5);
    }
}
