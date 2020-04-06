using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    [Min(0)]
    private int health = 0;
    [SerializeField]
    [Min(0)]
    private int maxHealth = 0;
    [SerializeField]
    [Min(0)]
    [Tooltip("If bonusHealthCap is less that maxHealth, it is set to maxHelath.")]
    private int bonusHealthCap;

    private List<Action<int, RaycastHit?>> damageEventCallbacks;
    private List<Action<int>> healEventCallbacks;

    private void Awake() {
        // Set health to max if left at 0 in inspector.
        if(this.health == 0) {
            this.health = this.maxHealth;
        }

        if(this.bonusHealthCap == 0 || this.bonusHealthCap < this.maxHealth) {
            this.bonusHealthCap = this.maxHealth;
        }


        this.damageEventCallbacks = new List<Action<int, RaycastHit?>>();
        this.healEventCallbacks = new List<Action<int>>();
    }

    public void subscribeToDamageEvent(Action<int, RaycastHit?> func) {
        this.damageEventCallbacks.Add(func);
    }

    public void subscribeToHealEvent(Action<int> func) {
        this.healEventCallbacks.Add(func);
    }

    public int getMaxHealth() {
        return this.maxHealth;
    }

    public int getHealth() {
        return this.health;
    }

    private int func(int i, bool exceedMax = false) {
        return Mathf.Clamp(i, 0, exceedMax ? this.bonusHealthCap : this.maxHealth);
    }

    /// <summary>
    /// Sets the Health.  this will not trigger any callback events.
    /// If you wish to trigger them, use the heal() or damage() methods instead.
    /// </summary>
    public void setHealth(int amount, bool exceedMax = false) {
        this.health = this.func(amount, exceedMax = false);
    }

    public void heal(int amount, bool exceedMax = false) {
        int i = this.func(this.health + amount, exceedMax);
        this.health = i;

        foreach(Action<int> callback in this.healEventCallbacks) {
            callback(amount);
        }
    }

    /// <summary>
    /// Damages the health object.  Returns true if it is now "dead" (health = 0).
    /// </summary>
    public bool damage(int amount, RaycastHit? hit = null) {
        int i = this.func(this.health - amount);
        this.health = i;

        foreach(Action<int, RaycastHit?> callback in this.damageEventCallbacks) {
            callback(amount, hit);
        }

        return this.isDead();
    }

    public bool isDead() {
        return this.health <= 0;
    }
}
