public class ActionButtonChild : ActionButton {

    public const int CHILD_ID = -1;

    public ActionButtonParent parentActionButton;
    /// <summary>
    /// The index of this button within the parent's child button list.
    /// </summary>
    public int index;

    public ActionButtonChild(string actionName) : base(actionName, CHILD_ID) { }

    public override int getMask() {
        return this.parentActionButton.getMask();
    }
}
