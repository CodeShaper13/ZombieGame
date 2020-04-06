using UnityEngine;

public class Billboard : MonoBehaviour {

    private Camera mainCamera;

    private void Start() {
        this.mainCamera = Camera.main;
    }

    private void LateUpdate() {
        if(this.mainCamera != null) {
            this.transform.rotation = this.mainCamera.transform.rotation;
        }
    }
}