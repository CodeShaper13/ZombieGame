using UnityEngine;

public class SelectionBox : MonoBehaviour {

    private RectTransform selectionRect;
    private Vector2 boxStartPos;
    private Vector2 boxEndPos;

    private void Start() {
        this.selectionRect = this.GetComponent<RectTransform>();
    }

    public void updateRect() {
        if(this.func()) {
            this.updateRectPosition();
        }
        else {
            //this.reset();
        }

        if(Input.GetMouseButton(2)) {
            if(Input.GetMouseButtonDown(2)) {
                this.boxStartPos = Input.mousePosition;
            }
            else {
                this.boxEndPos = Input.mousePosition;
            }
        }
        else {
            // Handle the case where the player had been drawing a box but has now released.
            if(this.func()) {
                //HandleUnitSelection();
            }
            this.reset();
        }
    }

    public void updateRectPosition() {
        Vector2 middle = (this.boxStartPos + this.boxEndPos) / 2f;
        this.selectionRect.position = middle;

        //Set the size of the square
        this.selectionRect.sizeDelta = new Vector2(
            Mathf.Abs(this.boxStartPos.x - this.boxEndPos.x),
            Mathf.Abs(this.boxStartPos.y - this.boxEndPos.y));
    }

    public bool func() {
        return this.boxStartPos != Vector2.zero && this.boxEndPos != Vector2.zero;
    }

    public void reset() {
        this.boxStartPos = Vector2.zero;
        this.boxEndPos = Vector2.zero;
        this.selectionRect.position = Vector2.zero;
        this.selectionRect.sizeDelta = Vector2.zero;
    }
}
