using UnityEngine;
using UnityEngine.UI;

public class GraphicColorChange : MonoBehaviour {

    [SerializeField]
    [Tooltip("")]
    private bool ignorePause = false;
    [SerializeField]
    [Tooltip("")]
    private bool startOnAwake = true;
    [SerializeField]
    [Tooltip("")]
    private float delay = 0f;
    [SerializeField]
    [Tooltip("")]
    private float fadeSpeed = 1f;

    [SerializeField]
    private Color targetColor = Color.white;
    [SerializeField]
    private bool startWithTarget = false;

    private Color color1;
    private Color color2;
    private float timer;
    private bool running;

    [SerializeField]
    private Graphic graphic;

    private void Awake() {
        this.graphic = this.GetComponent<Graphic>();

        if(this.startOnAwake) {
            this.start();
        }
    }

    private void Update() {
        if(this.isRunning() && (this.ignorePause || !Pause.isPaused())) {
            this.timer += Time.deltaTime;

            if(this.graphic != null) {

                this.graphic.color = Color.Lerp(
                    this.color1,
                    this.color2,
                    this.timer * this.fadeSpeed);
            }
        }
    }

    public void start() {
        this.running = true;

        if(this.graphic != null) {
            if(this.startWithTarget) {
                this.color1 = this.targetColor;
                this.color2 = this.graphic.color;
            }
            else {
                this.color1 = this.graphic.color;
                this.color2 = this.targetColor;
            }

            this.graphic.color = this.color1;
        }

        this.timer = -this.delay;
    }

    public bool isRunning() {
        return this.running;
    }
}
