using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMaps : MonoBehaviour {

    public static TileMaps singleton;

    public Tilemap floorMap;
    public Tilemap objectMap;
    public Tilemap decalMap;

    private Grid grid;

    private Vector2Int combinedOrigin;
    private Vector2Int combinedSize;

    private void Awake() {
        TileMaps.singleton = this;

        this.grid = this.GetComponent<Grid>();

        // Compute the combined origin and size from all of the map.
        foreach(Tilemap tm in new Tilemap[] { this.floorMap, this.objectMap, this.decalMap }) {
            if(tm != null) {
                Vector3Int origin = tm.origin;
                Vector3Int size = tm.size;

                if(tm.size.x > this.combinedSize.x) {
                    this.combinedSize.x = tm.size.x;
                }
                if(tm.size.y > this.combinedSize.y) {
                    this.combinedSize.y = tm.size.y;
                }

                if(tm.origin.x < this.combinedOrigin.x) {
                    this.combinedOrigin.x = tm.origin.x;
                }
                if(tm.origin.y < this.combinedOrigin.y) {
                    this.combinedOrigin.y = tm.origin.y;
                }
            }
        }
    }

    public Vector2Int worldToCell(Vector3 v) {
        Vector3Int vi = this.grid.WorldToCell(v);
        return new Vector2Int(vi.x, vi.y);
    }

    public Vector2Int getCombinedSize() {
        return this.combinedSize;
    }

    public Vector2Int getCombinedOrigin() {
        return this.combinedOrigin;
    }
}
