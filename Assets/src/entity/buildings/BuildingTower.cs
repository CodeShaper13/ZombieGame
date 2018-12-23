using UnityEngine;

public class BuildingTower : BuildingBase {

    private UnitBase target;
    private float fireCooldown;

    public override void onUpdate(float deltaTime) {
        base.onUpdate(deltaTime);

        this.fireCooldown -= deltaTime;

        if(this.target == null || !(this.target)) {
            this.findTarget();
        }

        if(this.target != null && this.fireCooldown <= 0) {
            this.map.fireProjectile(
                this.transform.position + new Vector3(0, 3.25f, 0),
                this,
                Constants.BUILDING_TOWER_DAMAGE,
                this.target);

            this.fireCooldown = Constants.BUILDING_TOWER_FIRE_SPEED;
        }
    }

    public override float getHealthBarHeight() {
        return 6.5f;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(3, 3);
    }

    /// <summary>
    /// Trys to find a target.
    /// </summary>
    private void findTarget() {
        if(this.target == null || this.target.isDead() || Vector3.Distance(this.transform.position, this.target.transform.position) >= Constants.BUILDING_TOWER_FIRE_RANGE) {
            this.target = this.findUnit();
        }
    }

    public override void colorObject() {
        this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = this.getTeam().getTeamColor();
    }

    private UnitBase findUnit() {
        UnitBase obj = null;
        float f = float.PositiveInfinity;
        foreach(SidedEntity s in this.map.findMapObjects(this.getTeam().predicateOtherTeam)) {
            if(s is UnitBase) {
                float dis = Vector3.Distance(this.transform.position, s.transform.position);
                if((dis < f) && dis < Constants.BUILDING_TOWER_SEE_RANGE) {
                    obj = (UnitBase)s;
                    f = dis;
                }
            }
        }
        return obj;
    }

    public override int getMaxHealth() {
        return 200;
    }

    public override BuildingData getData() {
        return Constants.BD_TOWER;
    }
}