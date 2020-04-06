using UnityEngine;

/// <summary>
/// Creates an outline effect around a sprite.  To enable/hide the effect, enable or disable the componenet.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class SpriteOutline : MonoBehaviour {

    [SerializeField]
    private Color outlineColor = Color.white;

    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        this.updateOutline(true);
    }

    private void OnDisable() {
        this.updateOutline(false);
    }

    private void Update() {
        this.updateOutline(true);
    }

    public void setColor(Color color) {
        this.outlineColor = color;
    }

    public Color getColor() {
        return this.outlineColor;
    }

    private void updateOutline(bool outline) {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        this.spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", this.outlineColor);
        this.spriteRenderer.SetPropertyBlock(mpb);
    }
}