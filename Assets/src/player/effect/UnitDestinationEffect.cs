using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitDestinationEffect : MonoBehaviour {

    [SerializeField]
    private Vector3 spriteOffset;
    [SerializeField]
    private float fadeTime = 1f;

    private SpriteRenderer sr;
    private float fadeProgress;

    private void Awake() {
        this.sr = this.transform.GetComponent<SpriteRenderer>();
        this.setAlpha(0);
    }

    private void Update() {
        if(!Pause.isPaused()) {
            this.fadeProgress -= Time.deltaTime;
            if(this.fadeProgress >= -1) {
                this.setAlpha(this.fadeProgress);
            }
        }
    }

    public void setPosition(Vector3 pos) {
        this.transform.position = pos + this.spriteOffset;
        this.setAlpha(1);
        this.fadeProgress = this.fadeTime;
    }

    private void setAlpha(float alpha) {
        this.sr.color = sr.color.setAlpha(alpha);
    }
}
