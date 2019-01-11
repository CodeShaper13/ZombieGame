using UnityEngine;

public class Projectile : MapObject {

    private const float PROJECTILE_SPEED = 12f;

    private int damage;
    private LivingObject target;
    /// <summary> Keeps track of who shot the projectile, used for statistics.  Null if shot by a building. </summary>
    private SidedEntity shooter;

    private void Update() {
        if(this.isServer) {
            if(this.target == null || !(this.target)) {
                this.destroyProjectile();
            }
            else {
                // Move projectile towards target.
                this.transform.LookAt(this.target.transform.position);
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.target.transform.position, Projectile.PROJECTILE_SPEED * Time.deltaTime);


                // Damge target if the projectile is close enough.
                if(MathHelper.inRange(this.transform.position, this.target.transform.position, 0.25f)) {
                    bool isDead = this.target.damage(this, this.damage);
                    if(isDead && shooter is UnitBase) { // Not true for buildings like the tower.
                        ((UnitBase)this.shooter).unitStats.unitsKilled.increase();
                    }
                    this.destroyProjectile();
                }
            }
        }
    }

    private void destroyProjectile() {
        GameObject.Destroy(this.gameObject);
    }

    /// <summary>
    /// Returns the object that shot this projectile.  This should never be null.
    /// </summary>
    [ServerSideOnly]
    public SidedEntity getShooter() {
        return this.shooter;
    }

    [ServerSideOnly]
    public void setProjectileInfo(SidedEntity shooter, int damage, LivingObject target) {
        this.shooter = shooter;
        this.damage = damage;
        this.target = target;
    }

    public override string getDisplayName() {
        return "Projectile";
    }
}