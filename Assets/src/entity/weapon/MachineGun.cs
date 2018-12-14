using UnityEngine;

class MachineGun : GunBase {

    public override float getBulletOffset() {
        return Random.Range(-5f, 5f);
    }

    public override int getClipSize() {
        return 24;
    }

    public override float getReloadTime() {
        return 4f;
    }

    public override float getShotTime() {
        return 0.3f;
    }

    public override bool isInRange(SidedEntity target) {
        return Vector3.Distance(
            this.owner.transform.position,
            target.transform.position) < 15;
    }

    public override void setHoldLocation(UnitBase s) {
        this.owner = s;
        this.transform.localPosition = new Vector3(0.32f, -0.35f, 0.58f);
        this.transform.localEulerAngles = new Vector3(90, -22f, 0);
    }
}
