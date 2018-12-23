using UnityEngine;

public static class Direction {

    public static readonly Vector3Int NORTH = new Vector3Int(0, 0, 1);
    public static readonly Vector3Int SOUTH = new Vector3Int(0, 0, -1);
    public static readonly Vector3Int EAST = new Vector3Int(1, 0, 0);
    public static readonly Vector3Int WEST = new Vector3Int(-1, 0, 0);
    public static readonly Vector3Int UP = Vector3Int.up;
    public static readonly Vector3Int DOWN = Vector3Int.down;

    public static readonly Vector3Int[] CARDINAL = new Vector3Int[] { NORTH, SOUTH, EAST, WEST };

    public static readonly Vector3Int[] ALL = new Vector3Int[] { NORTH, SOUTH, EAST, WEST, UP, DOWN };
}