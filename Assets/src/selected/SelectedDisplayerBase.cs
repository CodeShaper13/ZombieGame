using UnityEngine;

public abstract class SelectedDisplayerBase : MonoBehaviour {

    private Canvas canvas;

    protected Player player;

    public virtual void init(Player player) {
        this.player = player;

        this.canvas = this.GetComponent<Canvas>();

        this.setUIVisible(false);
    }

    public virtual void onUpdate() { }

    /// <summary>
    /// Sets if the UI is visible.
    /// </summary>
    public void setUIVisible(bool visible) {
        this.canvas.enabled = visible;
    }

    public abstract int getMask();

    public abstract void clearSelected();

    public abstract void callFunctionOn(ActionButton actionButton);
}
