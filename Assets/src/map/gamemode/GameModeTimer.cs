using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class GameModeTimer : GameModeBase {

    [SerializeField]
    protected float startingTime = 0f;
    [SerializeField]
    protected bool countDown = true;
    [SerializeField]
    [Tooltip("If enabled, hours will show up on the timer text.")]
    protected bool showHours = false;
    [SerializeField]
    protected bool winOnTimeUp;

    protected float currentTime;

    private bool isPaused = false;

    private Canvas uiCanvas;
    private Slider slider;
    private Text text;

    public override void init(MapBase map) {
        base.init(map);

        // Instantiate the prefab
        this.uiCanvas = GameObject.Instantiate(References.list.prefabGameModeTimer, this.transform).GetComponent<Canvas>();
        NetworkServer.Spawn(this.uiCanvas.gameObject);

        this.uiCanvas = this.GetComponentInChildren<Canvas>();
        this.slider = this.GetComponentInChildren<Slider>();
        this.text = this.GetComponentInChildren<Text>();

        // Setup the time bar
        this.slider.maxValue = this.startingTime;

        if(this.countDown) {
            this.currentTime = startingTime;
        } else {
            this.currentTime = 0f;
        }
    }

    public override void updateGameMode() {
        if(!this.isPaused) {
            if(this.countDown) {
                this.currentTime -= Time.deltaTime;
            } else {
                this.currentTime += Time.deltaTime;
            }
        }

        // Update UI
        this.slider.value = this.currentTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        if(this.showHours) {
            this.text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        } else {
            this.text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }

        this.slider.value = this.currentTime;

        // Check for a level win/lose
        if(this.countDown) {
            if(this.currentTime <= 0) {
                this.triggerLoss();
            }
        } else {
            if(this.currentTime >= startingTime) {
                this.triggerLoss();
            }
        }
    }

    private void func() {
        if(this.winOnTimeUp) {
            this.triggerWin();
        } else {
            this.triggerLoss();
        }
    }

    public void setUiVisability(bool visable) {
        this.uiCanvas.enabled = visable;
    }

    public void pause() {
        this.isPaused = true;
    }

    public void unPause() {
        this.isPaused = false;
    }
}
