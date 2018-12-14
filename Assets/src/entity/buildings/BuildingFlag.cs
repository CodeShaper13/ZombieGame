using UnityEngine;

public class BuildingFlag : BuildingBase {

    public Transform flagRotatePoint;
    public MeshRenderer flagCloth;

    private const float speed = 5f;
    private const float maxRotation = 6f;

    private void Update() {
        this.flagRotatePoint.rotation = Quaternion.Euler(
            0f,
            maxRotation * Mathf.Sin(Time.time * speed),
            0f);
    }

    public override void colorObject() {
        base.colorObject();

        this.flagCloth.material.color = this.getTeam().getTeamColor();
    }

    public override Vector3 getFootPos() {
        return this.transform.position + new Vector3(0, -0.5f, 0);
    }
}
