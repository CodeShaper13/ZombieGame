using UnityEngine;

public class Billboard : MonoBehaviour {

    private void LateUpdate() {
        Player pc = Player.localPlayer;
        if(pc != null) {
            this.transform.rotation = Player.localPlayer.cameraObj.transform.rotation;
        }
    }
}