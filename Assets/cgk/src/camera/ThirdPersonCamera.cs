using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    [SerializeField]
    private bool followPlayer;

    private IPlayer player;
    private Vector3 startingOffset;
    private Quaternion startingRot;

    private void Awake() {
        //this.player = GameObject.FindObjectOfType<IPlayer>();

        this.startingOffset = this.transform.position - this.player.getTransform().position;
        this.startingRot = this.transform.rotation;
    }

    private void Update() {
        this.transform.position = player.getTransform().position + this.startingOffset;
        this.transform.rotation = this.startingRot;
    }

    private enum CameraMoveType {
        DONT_MOVE = 0,
        USE_CINEMACHINE = 1,
        FOLLOW_PLAYER = 2,
    }
}
