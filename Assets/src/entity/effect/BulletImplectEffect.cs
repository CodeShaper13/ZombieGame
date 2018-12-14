using UnityEngine;

public class BulletEffect : MonoBehaviour {

    public GameObject particleFX;

    public void spawnHitFX(Vector3 position, Quaternion rotation) {
        GameObject g = GameObject.Instantiate(this.particleFX);
        g.transform.position = position;
        //g.transform.rotation = b.transform.rotation;// Quaternion.Euler(-b.transform.eulerAngles.x, b.transform.eulerAngles.y, b.transform.eulerAngles.z);
        g.transform.parent = this.transform;
    }
}
