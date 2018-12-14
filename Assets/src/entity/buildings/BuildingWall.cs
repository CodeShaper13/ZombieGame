using UnityEngine;

public class BuildingWall : BuildingBase {

    public Transform[] hidingSpots = new Transform[3];

    public override Vector3 getFootPos() {
        return this.transform.position;
    }
}
