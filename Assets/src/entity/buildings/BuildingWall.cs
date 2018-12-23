using UnityEngine;

public class BuildingWall : BuildingBase {

    public Transform[] hidingSpots = new Transform[3];

    public override BuildingData getData() {
        return Constants.BD_WALL;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(4f, 1f);
    }

    public override float getHealthBarHeight() {
        return 1f;
    }

    public override int getMaxHealth() {
        return 100;
    }

}
