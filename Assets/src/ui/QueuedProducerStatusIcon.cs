using UnityEngine;
using UnityEngine.UI;

public class QueuedProducerStatusIcon : MonoBehaviour {

    private Text text;

    private void Awake() {
        this.text = this.GetComponentInChildren<Text>();
    }

    public void setVisible(bool visible) {
        this.gameObject.SetActive(visible);
        this.setText(null);
    }

    public void setText(string text) {
        if(text == null) {
            this.text.text = string.Empty;
        }
        else {
            this.text.text = text;
        }
    }
}
