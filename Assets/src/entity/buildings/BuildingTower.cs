using fNbt;
using UnityEngine;

public class BuildingTower : BuildingBase {

    private UnitBase target;
    private float fireCooldown;

    public MeshRenderer middleRenderer;

    protected override void preformTask(float deltaTime) {
        base.preformTask(deltaTime);

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
        return 5f;
    }

    public override Vector2 getFootprintSize() {
        return new Vector2(3, 3);
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.fireCooldown = tag.getFloat("fireCooldown");
        MapObject obj = this.map.findMapObjectFromGuid<MapObject>(tag.getGuid("targetGuid"));
        if(obj is UnitBase) {
            this.target = (UnitBase)obj;
        }
        else {
            this.target = null;
        }
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("fireCooldown", this.fireCooldown);
        if(this.target != null) {
            tag.setTag("targetGuid", this.target.getGuid());
        }
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
        this.middleRenderer.material.color = this.getTeam().getColor();
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