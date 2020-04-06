using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {

    [SerializeField]
    private SpriteAnimationData data;

    private SpriteRenderer sr;
    private Vector2 posLastFrame;
    private float timeWalking;
    private int directionMoving;

    private void Awake() {
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if(!Pause.isPaused()) {
            Vector2 newPos = new Vector2(this.transform.position.x, this.transform.position.y);

            int dir = 0; // 0 = Dont change, 1 = Forwards, 2 = Backwards.

            if(newPos != this.posLastFrame) {
                this.timeWalking += Time.deltaTime;

                // Flip the sprite on the x axis depending on the direction it moved.
                if(newPos.x > posLastFrame.x) {
                    this.sr.flipX = false;
                } else if(newPos.x < posLastFrame.x) {
                    this.sr.flipX = true;
                }

                // Calculate the direction the sprite is moving.

                if(newPos.y > posLastFrame.y) {
                    dir = 2;
                    this.sr.sprite = this.data.unmovingBack;
                }
                else if(newPos.y < posLastFrame.y) {
                    dir = 1;
                    this.sr.sprite = this.data.unmovingFront;
                }
            } else {
                this.timeWalking = 0;
            }

            float f = this.timeWalking - ((int)this.timeWalking);
            int frame = (int)((this.timeWalking * 4) % 2);

            // Set the texture to use.
            if(dir == 0) {
                    this.sr.sprite = this.data.unmovingFront;
            }
            else if(dir == 1) {
                this.sr.sprite = this.data.getWalkingFrontSprite(frame);
            }
            else if(dir == 2) {
                this.sr.sprite = this.data.getWalkingBackSprite(frame);
            }


            this.posLastFrame = newPos;
        }
    }
}
