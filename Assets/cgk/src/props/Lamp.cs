using UnityEngine;

public class Lamp : MonoBehaviour, IClickable<object> {

    private Light lightSource;

    private void Awake() {
        this.lightSource = this.GetComponentInChildren<Light>();
    }

    public bool onClick(object clicker) {
        this.lightSource.enabled = !this.lightSource.enabled;
        return true;
    }
}
