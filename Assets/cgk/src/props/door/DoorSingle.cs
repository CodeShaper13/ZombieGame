using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DoorSingle : DoorBase {

    [HideInInspector]
    [SerializeField]
    private Quaternion openState = Quaternion.identity;
    [HideInInspector]
    [SerializeField]
    private Quaternion closedState = Quaternion.identity;

    [Tooltip("How many degrees this door moves per seconds while opening or closing.")]
    public float openSpeed = 150f;

    public override bool detectIfOpen() {
        return Quaternion.Angle(this.transform.localRotation, this.openState) < 5;
    }

    protected override void updateDoor() {
        Quaternion targ = this.isOpen ? this.openState : this.closedState;

        if(this.transform.localRotation != targ) {
            this.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, targ, this.openSpeed * Time.deltaTime);
        }
    }

    public override void setAsOpen() {
        this.transform.localRotation = this.openState;
    }

    public override void setAsClosed() {
        this.transform.localRotation = this.closedState;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(DoorSingle))]
    public class EditorDoorSingle : EditorDoorBase {

        public override void markAsOpen(DoorBase door) {
            this.openState.quaternionValue = door.transform.localRotation;
        }

        public override void markAsClosed(DoorBase door) {
            this.closedState.quaternionValue = door.transform.localRotation;
        }
    }
#endif
}
