using fNbt;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents a MapObject that is alive.  It has health and can be damaged.
/// </summary>
[RequireComponent(typeof(OutlineHelper))]
public abstract class LivingObject : MapObject {

    [SyncVar(hook = "hookOnChangeHealth")]
    private int currentHealth;
    private ProgressBar healthBar;
    private bool shouldShowHealth;

    [HideInInspector]
    public OutlineHelper outlineHelper;

    public override void onAwake() {
        base.onAwake();

        this.currentHealth = this.getMaxHealth();
    }

    public override void onUiInit() {
        base.onUiInit();

        this.outlineHelper = this.GetComponent<OutlineHelper>();
        this.outlineHelper.hideAll();

        this.shouldShowHealth = Main.DEBUG_HEALTH || !(this is HarvestableObject) || (this is SidedEntity && ((SidedEntity)this).getTeam() == Player.localPlayer.getTeam());

        this.hookOnChangeHealth(this.currentHealth);
    }

    /// <summary>
    /// Returns true if this object is "dead", it's health is 0.
    /// </summary>
    public bool isDead() {
        return this.currentHealth <= 0;
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
    [ServerSideOnly]
    public void setHealth(int amount) {
        int maxHealth = this.getMaxHealth();
        if(amount == -1) {
            amount = maxHealth;
        }
        this.currentHealth = Mathf.Clamp(amount, 0, maxHealth);
    }

    [ServerSideOnly]
    public bool damage(MapObject dealer, int amount) {
        // Make damage only taken on server.
        if(this.isServer) {
            this.setHealth(this.getHealth() - amount);
            if(this.isDead()) {
                this.map.removeMapObject(this);
                return true;
            }
            else {
                return false;
            }
        }

        return this.isDead();
    }

    [ServerSideOnly]
    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.currentHealth = tag.getInt("health");
    }

    [ServerSideOnly]
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
    [ServerSideOnly]
    public virtual void onDeath() { }

    public virtual bool shouldHealthbarBeShown() {
        return true;
    }

    [ClientSideOnly]
    private void hookOnChangeHealth(int newHealth) {
        this.currentHealth = newHealth;

        if(this.shouldShowHealth && this.shouldHealthbarBeShown()) {
            if(this.healthBar == null) {
                this.healthBar = ProgressBar.instantiateBar(
                    this.gameObject,
                    this.getHealthBarHeight(),
                    this.getMaxHealth(),
                    (curentValue, maxValue) => {
                        if(curentValue < (maxValue / 4)) {
                            return Colors.red;
                        }
                        else if(curentValue < (maxValue / 2)) {
                            return Colors.orange;
                        }
                        else {
                            return Colors.lime;
                        }
                    });
            }

            this.healthBar.updateProgressBar(newHealth);
            this.healthBar.setVisible(this.shouldHealthbarBeShown());
        }
    }
}
