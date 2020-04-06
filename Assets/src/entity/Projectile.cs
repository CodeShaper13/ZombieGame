using UnityEngine;

public class Projectile : MapObject {

    [SerializeField]
    [Min(0)]
    private float projectileSpeed = 1;
    [SerializeField]
    [Tooltip("How close the bullet needs to be to damage the target.")]
    [Min(0)]
    private float damageDistance = 0.1f;

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
                this.transform.Rotate(0, -90, 0);
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.target.transform.position, this.projectileSpeed * Time.deltaTime);
                
                // Damge target if the projectile is close enough.
                if(MathHelper.inRange(this.transform.position, this.target.transform.position, this.damageDistance)) {
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