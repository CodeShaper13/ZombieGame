using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour {

    public float secondsUntilDestruction = 1f;
    public bool ignorePause = false;

    private float timer;

    private void Update() {
        if(this.ignorePause || !Pause.isPaused()) {
            this.timer += Time.deltaTime;

            if(this.timer >= this.secondsUntilDestruction) {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
