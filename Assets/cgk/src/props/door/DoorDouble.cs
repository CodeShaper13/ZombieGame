using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class DoorDouble : DoorBase {

    public DoorDoubleCaller right;
    public DoorDoubleCaller left;

    [SerializeField]
    private Quaternion rightOpenState = Quaternion.identity;
    [SerializeField]
    private Quaternion rightClosedState = Quaternion.identity;
    [SerializeField]
    private Quaternion leftOpenState = Quaternion.identity;
    [SerializeField]
    private Quaternion leftClosedState = Quaternion.identity;

    public float openSpeed = 150f;

    public override bool detectIfOpen() {
        bool flag = Quaternion.Angle(this.right.transform.localRotation, this.rightOpenState) < 5;
        bool flag1 = Quaternion.Angle(this.left.transform.localRotation, this.leftOpenState) < 5;

        return flag || flag1;
    }

    public override void setAsClosed() {
        this.right.transform.localRotation = this.rightClosedState;
        this.left.transform.localRotation = this.leftClosedState;
    }

    public override void setAsOpen() {
        this.right.transform.localRotation = this.rightOpenState;
        this.left.transform.localRotation = this.leftOpenState;
    }

    protected override void updateDoor() {
        this.func(this.right.transform, this.rightOpenState, this.rightClosedState);
        this.func(this.left.transform, this.leftOpenState, this.leftClosedState);
    }

    public override void destroyDoor() {
        this.func01(this.right);
        this.func01(this.left);
    }

    public override void createNavTrigger() {
        BoxCollider trigger = this.gameObject.AddComponent<BoxCollider>();
        float doorHeight = Mathf.Max(boxColliders[0].size.y, boxColliders[1].size.y);
        trigger.center = new Vector3(
            (this.right.transform.localPosition.x + this.left.transform.localPosition.x) / 2,
            doorHeight / 2,
            0);
        trigger.size = new Vector3(
            boxColliders[0].size.x + boxColliders[1].size.x,
            doorHeight,
            Mathf.Max(boxColliders[0].size.z, boxColliders[1].size.z) + DoorBase.TRIGGER_BLOAT_SIZE);
        trigger.isTrigger = true;
    }

    private void func01(DoorDoubleCaller d) {
        d.GetComponent<MeshRenderer>().enabled = false;
        foreach(BoxCollider bc in d.GetComponents<BoxCollider>()) {
            bc.enabled = false;
        }
        GameObject.Destroy(d.gameObject, 2f);
    }

    private void func(Transform doorTrans, Quaternion openState, Quaternion closedState) {
        Quaternion targ = this.isOpen ? openState : closedState;

        if(doorTrans.localRotation != targ) {
            doorTrans.localRotation = Quaternion.RotateTowards(doorTrans.localRotation, targ, this.openSpeed * Time.deltaTime);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DoorDouble))]
    public class EditorDoorDouble : EditorDoorBase {

        protected SerializedProperty rightOpenState;
        protected SerializedProperty rightClosedState;
        protected SerializedProperty leftOpenState;
        protected SerializedProperty leftClosedState;

        private void OnEnable() {
            this.rightOpenState = this.serializedObject.FindProperty("rightOpenState");
            this.rightClosedState = this.serializedObject.FindProperty("rightClosedState");
            this.leftOpenState = this.serializedObject.FindProperty("leftOpenState");
            this.leftClosedState = this.serializedObject.FindProperty("leftClosedState");
        }

        public override void markAsClosed(DoorBase door) {
            this.rightClosedState.quaternionValue = ((DoorDouble)door).right.transform.localRotation;
            this.leftClosedState.quaternionValue = ((DoorDouble)door).left.transform.localRotation;
        }

        public override void markAsOpen(DoorBase door) {
            this.rightOpenState.quaternionValue = ((DoorDouble)door).right.transform.localRotation;
            this.leftOpenState.quaternionValue = ((DoorDouble)door).left.transform.localRotation;
        }
    }
#endif
}
