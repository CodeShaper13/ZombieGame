using UnityEngine;
using UnityEngine.Networking;

public class HitPoints : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;

    public bool destroyOnDeath;

    public void damage(int amount) {
        // Make damage only taken on server
        if (isServer) {
            this.currentHealth -= amount;
            if (currentHealth <= 0) {
                if (this.destroyOnDeath) {
                    Destroy(this.gameObject);
                }
                else {
                    currentHealth = maxHealth;

                    // called on the Server, but invoked on the Clients
                    RpcRespawn();
                }
            }
        }
    }

    void OnChangeHealth(int health) {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }
}