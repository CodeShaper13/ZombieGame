using UnityEngine;

public class BuildingCamp : BuildingBase {

    public override void colorObject() {
        this.transform.GetComponent<MeshRenderer>().material.color = this.getTeam().getColor();
    }

    public override float getHealthBarHeight() {
        return 2f;
    }

    public override BuildingData getData() {
        return Constants.BD_CAMP;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(4, 4);
    }
}
