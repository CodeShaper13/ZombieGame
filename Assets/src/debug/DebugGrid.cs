using UnityEngine;

public class DebugGrid : MonoBehaviour {

    // universal grid scale
    public float gridScale = 1f;

    // extents of the grid
    public int minX = -15;
    public int minY = -15;
    public int maxX = 15;
    public int maxY = 15;

    // nudges the whole grid rel
    public Vector3 gridOffset = Vector3.zero;

    // is this an XY or an XZ grid?
    public bool topDownGrid = true;

    // choose a colour for the gizmos
    public int gizmoMajorLines = 5;
    public Color gizmoLineColor = new Color(0.4f, 0.4f, 0.3f, 1f);

    private void Update() {
        if(!Main.DEBUG) {
            return;
        }

        int i = ((int)this.transform.position.x / 10) * 10;
        int j = ((int)this.transform.position.z / 10) * 10;
        Vector3 k = new Vector3(i, 0, j);

        // set colours
        Color dimColor = new Color(gizmoLineColor.r, gizmoLineColor.g, gizmoLineColor.b, 0.25f * gizmoLineColor.a);
        Color brightColor = Color.Lerp(Color.white, gizmoLineColor, 0.75f);

        Color drawColor;

        // draw the horizontal lines
        for(int x = minX; x < maxX + 1; x++) {
            // find major lines
            drawColor = (x % gizmoMajorLines == 0 ? gizmoLineColor : dimColor);
            if(x == 0) {
                drawColor = brightColor;
            }

            Vector3 pos1 = new Vector3(x, minY, 0) * gridScale;
            Vector3 pos2 = new Vector3(x, maxY, 0) * gridScale;

            // convert to topdown/overhead units if necessary
            if(topDownGrid) {
                pos1 = new Vector3(pos1.x, 0, pos1.y);
                pos2 = new Vector3(pos2.x, 0, pos2.y);
            }

            GLDebug.DrawLine((gridOffset + pos1) + k, (gridOffset + pos2) + k, drawColor, 0, true);
        }

        // draw the vertical lines
        for(int y = minY; y < maxY + 1; y++) {
            // find major lines
            drawColor = (y % gizmoMajorLines == 0 ? gizmoLineColor : dimColor);
            if(y == 0) {
                drawColor = brightColor;
            }

            Vector3 pos1 = new Vector3(minX, y, 0) * gridScale;
            Vector3 pos2 = new Vector3(maxX, y, 0) * gridScale;

            // convert to topdown/overhead units if necessary
            if(topDownGrid) {
                pos1 = new Vector3(pos1.x, 0, pos1.y);
                pos2 = new Vector3(pos2.x, 0, pos2.y);
            }

            GLDebug.DrawLine((gridOffset + pos1) + k, (gridOffset + pos2) + k, drawColor, 0, true);
        }
    }
}