using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class DoorSliding : DoorBase {

    [HideInInspector]
    public Vector3 closedState;
    [HideInInspector]
    public Vector3 openState;

    [Header("How fast the doors opens.  (This is units per second)")]
    public float openSpeed = 2.5f;

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.openState, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.closedState, 0.1f);
    }

    public override bool detectIfOpen() {
        return Vector3.Distance(this.transform.localPosition, this.openState) < 0.1f;
    }

    protected override void updateDoor() {
        Vector3 targ = (this.isOpen ? this.openState : this.closedState);

        if(this.transform.localPosition != targ) {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targ, this.openSpeed * Time.deltaTime);
        }
    }

    public override void setAsOpen() {
        this.transform.localPosition = this.openState;
    }

    public override void setAsClosed() {
        this.transform.localPosition = this.closedState;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DoorSliding))]
    public class EditorBarnDoor : EditorDoorBase {

        public override void OnInspectorGUI() {
            this.DrawDefaultInspector();

            DoorSliding door = (DoorSliding)this.target;

            if(GUILayout.Button("Set Open Pos (Green)")) {
                door.openState = door.transform.position;
            }
            if(GUILayout.Button("Set Closed Pos (Red)")) {
                door.closedState = door.transform.position;
            }
            if(GUILayout.Button("Detect Open State")) {
                door.isOpen = door.detectIfOpen();
            }
        }

        public override void markAsClosed(DoorBase door) {
            this.openState.vector3Value = door.transform.localPosition;
        }

        public override void markAsOpen(DoorBase door) {
            this.closedState.vector3Value = door.transform.localPosition;
        }
    }
#endif
}
