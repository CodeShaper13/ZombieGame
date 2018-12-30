using fNbt;
using UnityEngine;
using UnityEngine.Networking;

public abstract class LivingObject : MapObject {

    [SyncVar(hook = "hookOnChangeHealth")]
    private int currentHealth;
    private ProgressBar healthBar;

    public override void onAwake() {
        base.onAwake();

        this.healthBar = ProgressBar.instantiateBar(
            gameObject,
            this.getHealthBarHeight(),
            getMaxHealth(),
            (curentValue, maxValue) => {
                if(curentValue < (maxValue / 4)) {
                    return Colors.red;
                } else if(curentValue < (maxValue / 2)) {
                    return Colors.orange;
                } else {
                    return Colors.lime;
                }
            });
        this.currentHealth = getMaxHealth();
    }

    /// <summary>
    /// Returns true if this object is "dead", it's health is 0.
    /// </summary>
    public bool isDead() {
        return currentHealth <= 0;
    }

    /// <summary>
    /// Returns the health of the entity.  This is a simple getter;
    /// </summary>
    public int getHealth() {
        return this.currentHealth;
    }

    /// <summary>
    /// Should not be used to "damage" an object, as it does not record
    /// the damage to the stat list.
    /// Pass -1 to set max health.
    /// </summary>
    public void setHealth(int amount) {
        int maxHealth = this.getMaxHealth();
        if(amount == -1) {
            amount = maxHealth;
        }
        this.currentHealth = Mathf.Clamp(amount, 0, maxHealth);

        //if(this.shouldShowHealth && this.shouldHealthbarBeShown()) {
        //    if(this.healthBar == null) {
        //        this.healthBar = ProgressBar.instantiateHealthbar(this.gameObject, this.getHealthBarHeight(), this.getMaxHealth());
        //    }
        //    this.healthBar.updateProgressBar(amount);
        //    this.healthBar.setVisible(this.shouldHealthbarBeShown());
        //}
    }

    public bool damage(MapObject dealer, int amount) {
        // Make damage only taken on server.
        if(isServer) {
            this.setHealth(this.getHealth() - amount);
            if(isDead()) {
                this.map.removeEntity(this);
                return true;
            }
            else {
                return false;
            }
        }

        return this.isDead();
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.currentHealth = tag.getInt("health");
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("health", this.currentHealth);
    }

    /// <summary>
    /// Returns the maximum about of healt this Unit can have.
    /// </summary>
    public abstract int getMaxHealth();

    public abstract float getHealthBarHeight();

    public abstract float getSizeRadius();

    /// <summary>
    /// Called on the server side when this object is destroyed.
    /// </summary>
    public virtual void onDeath() { }

    private void hookOnChangeHealth(int newHealth) {
        this.currentHealth = newHealth;
        if(this.healthBar != null) {
            this.healthBar.updateProgressBar(newHealth);
        }
    }
}
