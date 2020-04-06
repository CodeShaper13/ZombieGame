using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ColorChangeOnHover : MonoBehaviour {

    [SerializeField]
    private Color highlightColor = Color.yellow;

    private Text text;
    private Color startingColor;

    private void Start() {
        this.text = this.GetComponent<Text>();
        this.startingColor = this.text.color;

        this.func(EventTriggerType.PointerEnter, true);
        this.func(EventTriggerType.PointerExit, false);
    }

    private void func(EventTriggerType ett, bool b) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = ett;
        entry.callback.AddListener((eventData) => { this.callback_toggleHighlight(b); });

        EventTrigger et = this.GetComponent<EventTrigger>();
        et.triggers.Add(entry);
    }

    public void callback_toggleHighlight(bool highlight) {
        this.text.color = highlight ? this.highlightColor : this.startingColor;
    }
}
