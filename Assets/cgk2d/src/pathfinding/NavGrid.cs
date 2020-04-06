using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

public class NavGrid : MonoBehaviour {

    [SerializeField]
    [Tooltip("If checked, diagonal nodes are considered neighbors.")]
    private bool includeDiagonals = true;

	private float nodeDiameter = 1;
    private Node[,] grid;
    private int gridSizeX, gridSizeY;

    public TileMaps tileMap;
    public Tilemap tmWalkableData;

    [Space]

    [SerializeField]
    private bool displayGridGizmos;

    private void Awake() {
        this.gridSizeX = this.tileMap.getCombinedSize().x;
        this.gridSizeY = this.tileMap.getCombinedSize().y;

        this.generateGrid();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

    /// <summary>
    /// Generates a grid of nodes for navigation.
    /// </summary>
    private void generateGrid() {
        this.grid = new Node[this.gridSizeX, this.gridSizeY];
        
        Vector2Int size = this.tileMap.getCombinedSize();
        Vector2Int orgin = this.tileMap.getCombinedOrigin();

        int orginX = orgin.x;
        int orginY = orgin.y;

        for(int x = 0; x < this.gridSizeX; x++) {
            for(int y = 0; y < this.gridSizeY; y++) {
                bool walkable = this.tmWalkableData.GetTile(new Vector3Int(x + orginX, y + orginY, 0)) == null;
                this.grid[x, y] = new Node(walkable, new Vector3(x + orginX + this.tmWalkableData.tileAnchor.x, y + orginY + this.tmWalkableData.tileAnchor.y), x, y, 0);
            }
        }
    }

    /// <summary>
    /// Returns all of the neighbors of the passed node.
    /// </summary>
	public List<Node> getNeighbors(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) {
					continue;
                }

                // Prevent diagonals from being added
                if(!this.includeDiagonals && Math.Abs(x) == Math.Abs(y)) {
                    continue;
                }

                int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < this.gridSizeX && checkY >= 0 && checkY < this.gridSizeY) {
                    neighbours.Add(this.grid[checkX, checkY]);
                }
			}
		}

		return neighbours;
	}

	public Node nodeFromWorldPoint(Vector3 worldPosition) {
        int x = (int)worldPosition.x - this.tileMap.getCombinedOrigin().x;
        int y = (int)worldPosition.y - this.tileMap.getCombinedOrigin().y;

        if(x < 0 || y < 0 || x >= this.grid.GetLength(0) || y >= this.grid.GetLength(1)) {
            // Out of bounds.
            return null;
        } else {
            return this.grid[x, y];
        }
	}
	
	private void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(1, 1, 1));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable ? Color.white : Color.red).setAlpha(0.5f);
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}

	[Serializable]
	public class TerrainType {
		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}