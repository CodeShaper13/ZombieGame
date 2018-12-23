public class ActionButtonParent : ActionButton {

    private readonly ActionButton[] childButtons;

    public ActionButtonParent(string actionName, int id, params ActionButton[] buttons) : base(actionName, id) {
        this.childButtons = buttons;
        for(int i = 0; i < this.childButtons.Length; i++) {
            ActionButtonChild child = (ActionButtonChild)buttons[i];
            child.parentActionButton = this;
            child.index = i;
        }
    }

    /// <summary>
    /// Returns an array of all the sub buttons.
    /// </summary>
    public ActionButton[] getChildButtons() {
        return this.childButtons;
    }
}
