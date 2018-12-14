using UnityEngine;
using UnityEngine.Networking;

public abstract class LivingObject : SidedEntity {

    /// <summary> May be null, this is destory is the object dies. </summary>
    private ProgressBar healthBar;

    [SyncVar(hook = "hookOnChangeHealth")]
    public int currentHealth;

    public override void onAwake() {
        base.onAwake();

        this.healthBar = ProgressBar.instantiateHealthbar(this.gameObject, 2.5f, this.getMaxHealth());
        this.currentHealth = this.getMaxHealth();
    }

    public override void onServerInit() {
        base.onServerInit();
    }

    /// <summary>
    /// Returns true if this object is "dead", it's health is 0.
    /// </summary>
    /// <returns></returns>
    public bool isDead() {
        return this.currentHealth <= 0;
    }

    public bool damage(int amount) {
        // Make damage only taken on server.
        if (this.isServer) {
            this.currentHealth = Mathf.Clamp(this.currentHealth - amount, 0, this.getMaxHealth());
            if (this.isDead()) {
                Map.instance.removeEntity(this);
            }
        }

        return this.isDead();
    }

    /// <summary>
    /// Returns the maximum about of healt this Unit can have.
    /// </summary>
    public abstract int getMaxHealth();

    /// <summary>
    /// Called on the server side when this object is destroyed.
    /// </summary>
    public virtual void onDeath() { }

    private void hookOnChangeHealth(int newHealth) {
        if(this.healthBar != null) {
            this.healthBar.updateProgressBar(newHealth);
        }
    }
}
