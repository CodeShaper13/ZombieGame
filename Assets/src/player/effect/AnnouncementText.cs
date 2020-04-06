using UnityEngine;
using UnityEngine.UI;

public class AnnouncementText : MonoBehaviour {

    private Text text;
    /// <summary> Time in seconds until the announcement text should vanish. </summary>
    private float timer;

    private void Awake() {
        this.text = this.GetComponent<Text>();
    }

    private void Update() {
        if(!Pause.isPaused()) {
            if(this.timer > 0) {
                this.timer -= Time.deltaTime;
                if(this.timer <= 0) {
                    this.text.text = string.Empty;
                }
            }
        }
    }

    /// <summary>
    /// Shows the passed text as an announcement that will disappear after time.
    /// </summary>
    public void showAnnouncement(string msg, float duration) {
        this.text.text = msg;
        this.timer = duration;
    }
}
