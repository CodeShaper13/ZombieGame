using UnityEngine;
using UnityEngine.UI;

public abstract class GuiBase : MonoBehaviour {

    [SerializeField]
    [Header("The Button that when clicked closes the gui")]
    protected Button closeButton;

    private void Awake() {
        if(this.closeButton != null) {
            this.closeButton.onClick.AddListener(GuiManager.closeTopGui);
        }

        this.onGuiInit();

        /*
        #if UNITY_EDITOR
            // While in dev, it's possible someone forgot to disable the GUI GameObject.
            this.gameObject.SetActive(false);
            Logger.logWarning("Someone forgot to disable a GUI GameObject...");
        #endif
        */
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
