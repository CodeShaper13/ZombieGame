using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    private int maxProgress = 1;
    [SerializeField]
    private RectTransform rect;
    private float originalX;
    private Image img;
    private Canvas canvas;

    private Func<int, int, Color> colorFunction;

    public static ProgressBar instantiateBar(GameObject holderObj, float height, int maxValue, Func<int, int, Color> colorFunction) {
        Vector3 pos = holderObj.transform.position + new Vector3(0, height, 0);
        GameObject obj = GameObject.Instantiate(References.list.prefabHealthBarEffect, pos, Quaternion.identity);
        ProgressBar progressBar = obj.GetComponent<ProgressBar>();
        progressBar.transform.SetParent(holderObj.transform);
        progressBar.gameObject.name = "ProgressBarCanvas";
        progressBar.maxProgress = maxValue;
        progressBar.colorFunction = colorFunction;
        progressBar.updateProgressBar(0);
        return progressBar;
    }

    private void Awake() {
        this.originalX = this.rect.sizeDelta.x;
        this.canvas = this.GetComponent<Canvas>();
        this.img = this.rect.GetComponent<Image>();
    }

    /// <summary>
    /// Returns true if object is "dead" (it's health is 0 or less).
    /// </summary>
    public void updateProgressBar(int amount) {
        float f = this.originalX / this.maxProgress;
        this.rect.sizeDelta = new Vector2(amount * f, this.rect.sizeDelta.y);

        this.img.color = this.colorFunction.Invoke(amount, this.maxProgress);
    }

    public void setVisible(bool visible) {
        this.canvas.enabled = visible;
    }
}
