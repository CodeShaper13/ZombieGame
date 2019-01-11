using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitDestinationEffect : MonoBehaviour {

    private const float HEIGHT = 0.1f;

    private SpriteRenderer sr;
    private float f;

    private void Awake() {
        this.sr = this.transform.GetComponent<SpriteRenderer>();
        this.transform.position.setY(HEIGHT);
        this.setAlpha(0);
    }

    private void Update() {
        if(!Main.instance().isPaused()) {
            this.f -= Time.deltaTime;
            if(this.f >= 0) {
                this.setAlpha(this.f);
            }
        }
    }

    public void setPosition(Vector3 pos) {
        this.transform.position = pos.setY(HEIGHT);
        this.setAlpha(1);
        this.f = 2f;
    }

    private void setAlpha(float alpha) {
        this.sr.color = sr.color.setA(alpha);
    }
}
