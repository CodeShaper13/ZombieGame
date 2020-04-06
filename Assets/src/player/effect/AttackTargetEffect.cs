using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AttackTargetEffect : MonoBehaviour {

    [SerializeField]
    private Vector2 targetOffset;
    [SerializeField]
    private float timeVisible;

    private SpriteRenderer sr;
    private float f;
    private SidedEntity target;

    private void Awake() {
        this.sr = this.transform.GetComponent<SpriteRenderer>();
        this.setAlpha(0);
    }

    private void Update() {
        if(!Pause.isPaused()) {
            if(Util.isAlive(this.target)) {
                this.f -= Time.deltaTime;
                this.centerAroundTarget();
                if(this.f >= -1) {
                    this.setAlpha(this.f);
                }
            } else {
                this.setAlpha(0);
                this.f = -10f;
            }
        }
    }

    public void setTarget(SidedEntity target) {
        this.target = target;
        this.centerAroundTarget();
        this.setAlpha(1);
        this.f = this.timeVisible;
    }

    private void setAlpha(float alpha) {
        this.sr.color = sr.color.setAlpha(alpha);
    }

    private void centerAroundTarget() {
        this.transform.position = this.target.transform.position + new Vector3(this.targetOffset.x, this.targetOffset.y);
    }
}
