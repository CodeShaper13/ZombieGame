using UnityEngine;

// Thanks to https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/

public class SelectionBox : MonoBehaviour {

    private Player player;
    private Texture2D whiteTexture;
    private bool isSelecting = false;
    private Vector3 mousePosition1;

    public void init(Player player) {
        this.player = player;

        this.whiteTexture = new Texture2D(1, 1);
        this.whiteTexture.SetPixel(0, 0, Color.white);
        this.whiteTexture.Apply();
    }
    
    public void updateRect() {
        // If we press the left mouse button, save mouse location and begin selection
        if(Input.GetMouseButtonDown(2)) {
            this.isSelecting = true;
            this.mousePosition1 = Input.mousePosition;
        }
        // If we let go of the left mouse button, end selection
        if(Input.GetMouseButtonUp(2)) {
            this.isSelecting = false;
        }

        foreach(MapObject obj in Map.instance.mapObjects) {
            if(obj is UnitBase) {
                if(isWithinSelectionBounds(obj)) {
                    this.player.selectedParty.tryAdd((UnitBase)obj);
                }
            }
        }
    }

    private void OnGUI() {
        if(this.isSelecting) {
            // Create a rect from both mouse positions
            var rect = this.getScreenRect(this.mousePosition1, Input.mousePosition);
            this.drawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            this.drawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    private Rect getScreenRect(Vector3 screenPosition1, Vector3 screenPosition2) {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    private void drawScreenRect(Rect rect, Color color) {
        GUI.color = color;
        GUI.DrawTexture(rect, whiteTexture);
        GUI.color = Color.white;
    }

    private void drawScreenRectBorder(Rect rect, float thickness, Color color) {
        // Top
        this.drawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        this.drawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        this.drawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        this.drawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    private Bounds getViewportBounds(Vector3 screenPosition1, Vector3 screenPosition2) {
        Vector3 v1 = this.player.playerCamera.ScreenToViewportPoint(screenPosition1);
        Vector3 v2 = this.player.playerCamera.ScreenToViewportPoint(screenPosition2);
        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);
        min.z = this.player.playerCamera.nearClipPlane;
        max.z = this.player.playerCamera.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    private bool isWithinSelectionBounds(MapObject mapObj) {
        if(!isSelecting) {
            return false;
        }
        Bounds viewportBounds = this.getViewportBounds(mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(this.player.playerCamera.WorldToViewportPoint(mapObj.transform.position));
    }
}