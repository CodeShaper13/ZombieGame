using System;
using UnityEngine;

public class RocketGun : GunBase {

    public override float getBulletOffset() {
        return 0.0f;
    }

    public override int getClipSize() {
        return 1;
    }

    public override float getReloadTime() {
        return 1.0f;
    }

    public override float getShotTime() {
        return 0.0f;
    }

    public override bool isInRange(SidedEntity target) {
        return Vector3.Distance(this.owner.transform.position, target.transform.position) < 15;
    }

    public override void setHoldLocation(UnitBase s) {
        this.owner = s;
        this.transform.localPosition = new Vector3(0.42f, -0.4f, 0.3f);
        this.transform.localEulerAngles = new Vector3(90, -168, 0);
    }
}
