using UnityEngine;

public class FogOfWarRevealer : MonoBehaviour {

    [SerializeField]
    [Min(0)]
    private int revealDistance;

    private FogOfWar fow;
    private Vector2Int posLastFrame;

    private void Awake() {
        this.fow = GameObject.FindObjectOfType<FogOfWar>();
        if(this.fow == null) {
            Debug.LogWarning("No FogOfWar Component could be found!  Is this an error?");
        }
    }

    private void Start() {
        this.liftFog();
    }

    private void Update() {
        Vector2Int currentPos = this.getPos();
        if(currentPos != this.posLastFrame) {
            this.liftFog();
        }

        this.posLastFrame = currentPos;
    }

    private void liftFog() {
        if(this.fow != null) {
            this.fow.liftFog(this.getPos(), this.revealDistance);
        }
    }

    private Vector2Int getPos() {
        return new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);
    }
}
