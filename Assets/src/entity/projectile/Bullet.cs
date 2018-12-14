using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MapObject {

    public override Vector3 getFootPos() {
        return this.transform.position;
    }

    private void OnCollisionEnter(Collision collision) {
        if(this.isServer) {
            LivingObject hitObject = collision.gameObject.GetComponent<LivingObject>();
            if (hitObject != null) {
                if (!hitObject.isDead()) {
                    hitObject.damage(10);

                    //if (nowDead) {
                    //    Rigidbody rb = hitObject.gameObject.GetComponent<Rigidbody>();
                    //    rb.isKinematic = false;
                    //    hitObject.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
                    //    rb.AddForce(this.transform.forward, ForceMode.Impulse);
                    //}
                }

                // Only show the partical effect on the client side.
                //this.RpcTryBulletEffect(collision.gameObject, collision.contacts[0].point);
            }

            // Destory the bullet if it hits something.
            GameObject.Destroy(this.gameObject);
        }
    }

    [ClientRpc]
    private void RpcTryBulletEffect(GameObject hitObject, Vector3 hitPoint) {
        BulletEffect be = hitObject.GetComponent<BulletEffect>();
        if (be != null) {
            be.spawnHitFX(hitPoint, this.transform.rotation);
            be.transform.parent = hitObject.transform;
        }
    }
}