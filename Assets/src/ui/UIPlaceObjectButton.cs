using UnityEngine;
using UnityEngine.UI;

public class UIPlaceObjectButton {

    private Button button;
    private Text text;
    public PlaceableObject placeableObj;

    public UIPlaceObjectButton(Button button, PlaceableObject obj) {
        this.button = button;
        this.text = this.button.GetComponentInChildren<Text>();
        this.placeableObj = obj;

        this.setText(this.placeableObj == null ? "NULL" : this.placeableObj.displayText);
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

    public void setSize(float size) {
        this.button.transform.localScale = new Vector3(size, size, size);
    }
}
