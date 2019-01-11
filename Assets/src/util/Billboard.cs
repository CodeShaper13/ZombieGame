using UnityEngine;

public class Billboard : MonoBehaviour {

    private void LateUpdate() {
        Player pc = Player.localPlayer;
        if(pc != null) {
            this.transform.rotation = pc.playerCamera.transform.rotation;
        }
    }
}