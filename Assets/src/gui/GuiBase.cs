using UnityEngine;
using UnityEngine.UI;

public abstract class GuiBase : MonoBehaviour {

    [SerializeField]
    [Header("The Button that when clicked closes the gui")]
    protected Button closeButton;

    private void Awake() {
        if(this.closeButton != null) {
            this.closeButton.onClick.AddListener(GuiManager.closeCurrentGui);
        }

        this.onGuiInit();
    }

    private void OnEnable() {
        this.onGuiOpen();
    }

    private void OnDisable() {
        this.onGuiClose();
    }

    public virtual void onGuiInit() { }

    public virtual void onGuiOpen() { }

    public virtual void onGuiClose() { }
}
