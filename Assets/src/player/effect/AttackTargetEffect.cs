using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AttackTargetEffect : MonoBehaviour {

    private const float HEIGHT = 0.1f;

    private SpriteRenderer sr;
    private float f;
    private SidedEntity target;

    private void Awake() {
        this.sr = this.transform.GetComponent<SpriteRenderer>();
        this.transform.position.setY(HEIGHT);
        this.setAlpha(0);
    }

    private void Update() {
        if(!Main.instance().isPaused()) {
            if(Util.isAlive(this.target)) {
                this.f -= Time.deltaTime;
                this.func();
                if(this.f > 0) {
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
        float size = target.getSizeRadius() * 2 * 1.5f;
        this.transform.localScale = new Vector3(size, size, size);
        this.func();
        this.setAlpha(1);
        this.f = 2f;
    }

    private void setAlpha(float alpha) {
        this.sr.color = sr.color.setA(alpha);
    }

    private void func() {
        this.transform.position = target is UnitBase ? ((UnitBase)target).getFootPos() : target.getPos().setY(HEIGHT);
    }
}
