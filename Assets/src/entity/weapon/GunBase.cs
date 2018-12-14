using UnityEngine;
using UnityEngine.Networking;

public abstract class GunBase : NetworkBehaviour {

    protected float timer;
    protected bool isReloading;
    protected int remainingAmmo;

    protected UnitBase owner;
    public ParticleSystem ps;

    private void Awake() {
        this.remainingAmmo = this.getClipSize();
    }

    public void updateGun() {
        this.timer -= Time.deltaTime;

        if(this.isReloading) {
            if (this.timer <= 0) {
                this.isReloading = false;
            }
        }
    }

    public void tryFire() {
        if(!this.isReloading && this.timer <= 0) {
            this.fireWeapon();
        }
    }

    public abstract float getBulletOffset();

    /// <summary>
    /// Returns the number of shots the gun gets between reloads.
    /// </summary>
    public abstract int getClipSize();

    public abstract float getShotTime();


    /// <summary>
    /// Returns how long in seconds it takes to reload this gun.
    /// </summary>
    public abstract float getReloadTime();

    public abstract void setHoldLocation(UnitBase s);

    /// <summary>
    /// Returns true if the passed target is in the range of this weapon.
    /// </summary>
    public abstract bool isInRange(SidedEntity target);

    public virtual void fireWeapon() {
        GameObject bullet = GameObject.Instantiate(
            Registry.projectileBullet.getPrefab(),
            this.owner.head.position,
            Quaternion.identity);
        bullet.transform.position += this.owner.head.forward;
        bullet.GetComponent<Rigidbody>().velocity = this.owner.head.forward * 6;

        // Spawn the bullet on the Clients.
        NetworkServer.Spawn(bullet);

        GameObject.Destroy(bullet, 5.0f);

        this.timer = this.getShotTime();
        this.remainingAmmo -= 1;
        if (this.remainingAmmo <= 0) {
            this.isReloading = true;
            this.remainingAmmo = this.getClipSize(); ;
            this.timer = this.getReloadTime();
        }

        //this.RpcPlaySmokeEffect();
    }

    public bool lookingAtEnemy() {
        RaycastHit hit;
        if (Physics.Raycast(this.owner.head.position, this.owner.head.forward, out hit)) {
            UnitBase s = hit.transform.GetComponentInParent<UnitBase>();
            if(s != null && s.getTeam() != this.owner.getTeam()) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    /// <summary>
    /// Plays the smoke effect on the client.
    /// </summary>
    [ClientRpc]
    private void RpcPlaySmokeEffect() {
        this.ps.Play();
    }
}
