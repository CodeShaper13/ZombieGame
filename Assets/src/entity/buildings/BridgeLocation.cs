using UnityEngine;

[ExecuteAlways]
public class BridgeLocation : MonoBehaviour {

    public float length;

    [SerializeField]
    private Transform bridgeStart;
    [SerializeField]
    private Transform bridgeEnd;
    [SerializeField]
    private BoxCollider bc;

    private void Update() {
        if(Application.IsPlaying(gameObject)) {
            // Play logic
        } else {
            float y = this.transform.position.y;
            //float z = this.transform.position.z;
            this.bridgeStart.position = this.bridgeStart.position.setY(y);//.setZ(z);
            this.bridgeEnd.position = this.bridgeEnd.position.setY(y);//.setZ(z);

            this.length = this.getLength();

            Vector3 v = this.getMidpoint();
            this.bc.center = v;
            this.bc.size = new Vector3(this.length, 1f, 1f);
        }
    }

    public Vector3 getMidpoint() {
        return (this.bridgeStart.position + this.bridgeEnd.position) / 2;
    }

    /// <summary>
    /// Returns the length of the bridge.
    /// </summary>
    public float getLength() {
        return Vector3.Distance(this.bridgeStart.position, this.bridgeEnd.position);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Colors.gray;

        Gizmos.DrawLine(this.bridgeStart.position, this.bridgeEnd.position);

        float f = 0.25f;
        Vector3 v = new Vector3(f, f, f);
        Gizmos.DrawCube(this.bridgeStart.position, v);
        Gizmos.DrawCube(this.bridgeEnd.position, v);

        Gizmos.color = Colors.cyan;
        Gizmos.DrawSphere(this.transform.position, 0.3f);
    }
}
