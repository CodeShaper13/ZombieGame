using UnityEngine;

public class LookHelper : MonoBehaviour {

    private Vector3? targetPos = null;
    private Transform targetTransform = null;

    private Animator anim;

    private void Awake() {
        this.anim = this.GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        if(!Pause.isPaused()) {
            if(this.anim.enabled) {
                if(this.targetTransform != null) {
                    Vector3? pos = this.getPos();
                    if(pos != null) {
                        this.anim.SetLookAtWeight(1, 0, 0.5f, 0.5f, 0.7f);
                        this.anim.SetLookAtPosition((Vector3)pos);
                    }
                } else {
                    this.anim.SetLookAtWeight(0, 0, 0, 0, 0);
                }
            }
        }
    }

    public void setTargetLook(Vector3? look) {
        this.resetLook();
        this.targetPos = look;
    }

    public void setTargetLook(Transform target) {
        this.resetLook();
        this.targetTransform = target;
    }

    public void resetLook() {
        this.targetPos = null;
        this.targetTransform = null;
    }

    private Vector3? getPos() {
        if(this.targetTransform != null) {
            return this.targetTransform.position;
        } else {
            return this.targetPos;
        }
    }
}
