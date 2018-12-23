using UnityEngine;

public class BuildingFlag : BuildingBase {

    public Transform flagRotatePoint;
    public MeshRenderer flagCloth;

    private const float speed = 5f;
    private const float maxRotation = 6f;

    private void Update() {
        this.flagRotatePoint.rotation = Quaternion.Euler(
            0f,
            (maxRotation * Mathf.Sin(Time.time * speed)) + 90,
            0f);
    }

    public override void colorObject() {
        base.colorObject();

        this.flagCloth.material.color = this.getTeam().getTeamColor();
    }

    public override int getMaxHealth() {
        return 100;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(0.2f, 0.2f);
    }

    public override float getHealthBarHeight() {
        return 4.5f;
    }

    public override BuildingData getData() {
        return Constants.BD_CAMP;
    }
}
