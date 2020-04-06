using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour {

    [SerializeField]
    private Tilemap fogOfWarTileMap = null;
    [Min(1)]
    [SerializeField]
    private int width = 1;
    [Min(1)]
    [SerializeField]
    private int height = 1;
    [SerializeField]
    private TileBase tileFog = null;
    [SerializeField]
    [Tooltip("Optional.  If null, no edge tiles are placed.")]
    private TileBase tileFogEdge = null;

    private void Awake() {
        this.fillWithFog();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(this.width, this.height, 0));
    }

    public void liftFog(Vector2Int currentPos, int revealDistance) {
        // Remove fog tiles.
        for(int x = -revealDistance; x <= revealDistance; x++) {
            for(int y = -revealDistance; y <= revealDistance; y++) {
                this.fogOfWarTileMap.SetTile(new Vector3Int(currentPos.x + x, currentPos.y + y, 0), null);
            }
        }

        // Update borader tiles, only if the boarder is set.
        if(this.tileFogEdge != null) {
            Vector2Int[] neighbors = new Vector2Int[] {
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1) };
            int disPlus1 = revealDistance + 1;
            for(int x = -disPlus1; x <= disPlus1; x++) {
                for(int y = -disPlus1; y <= disPlus1; y++) {
                    // Continue if the pos is not on the edge
                    if(!(x == -disPlus1 || y == -disPlus1 || x == disPlus1 || y == disPlus1)) {
                        continue;
                    }


                    Vector3Int v3 = new Vector3Int(x + currentPos.x, y + currentPos.y, 0);
                    if(this.fogOfWarTileMap.GetTile(v3) != null) { // Tile is fog.
                                                                   //foreach(Vector2Int v in neighbors) {
                                                                   //    Vector3Int neighborPos = new Vector3Int(x + currentPos.x + v.x, y + currentPos.y + v.y, 0);

                        //    // If one of the neightbor tiles is air, make this an edge tile.
                        //    if(this.fogOfWarTileMap.GetTile(neighborPos) == null) {
                        this.fogOfWarTileMap.SetTile(v3, this.tileFogEdge);
                        //        continue;
                        //    }
                        //}
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns true if there is fog at the passed tile.
    /// </summary>
    public bool isFogPresent(int x, int y) {
        return this.fogOfWarTileMap.GetTile(new Vector3Int(x, y, 0)) != null;
    }

    /// <summary>
    /// Fills the entire map with fog.
    /// </summary>
    public void fillWithFog() {
        this.fillMap(this.tileFog);
    }

    /// <summary>
    /// Clears the entire map of fog.
    /// </summary>
    public void clearAllFog() {
        this.fillMap(null);
    }

    private void fillMap(TileBase tile) {
        int w = this.width / 2;
        int h = this.height / 2;
        for(int x = -w; x <= w; x++) {
            for(int y = -h; y <= h; y++) {
                this.fogOfWarTileMap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
