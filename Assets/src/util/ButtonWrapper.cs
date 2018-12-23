using UnityEngine.UI;

/// <summary>
/// Wrapper for both a button and it's text.
/// </summary>
public class ButtonWrapper {

    public Button button;
    private Text text;

    public ButtonWrapper(Button button) {
        this.button = button;
        this.text = this.button.GetComponentInChildren<Text>();
    }

    public void setText(string text) {
        this.text.text = text;
    }

    /// <summary>
    /// Sets if the button shows up on the screen.
    /// </summary>
    public void setVisible(bool value) {
        this.button.gameObject.SetActive(value);
    }

    /// <summary>
    /// Sets if the button is enabled and can be clicked.
    /// </summary>
    public void setInteractable(bool interactable) {
        this.button.interactable = interactable;
    }
}
