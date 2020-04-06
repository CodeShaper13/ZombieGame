using UnityEngine;

public class Direction {

    public const int N_MASK = 1;
    public const int E_MASK = 2;
    public const int S_MASK = 4;
    public const int W_MASK = 8;
    public const int U_MASK = 16;
    public const int D_MASK = 32;
    /// <summary> The y plane, N E S W. </summary>
    public const int Y_MASK = N_MASK | E_MASK | S_MASK | W_MASK;
    /// <summary> The y plane, N E S W and up, U. </summary>
    public const int ALL = 63;

    public const int NONE_ID = 0;
    public const int NORTH_ID = 1;
    public const int EAST_ID = 2;
    public const int SOUTH_ID = 3;
    public const int WEST_ID = 4;
    public const int UP_ID = 5;
    public const int DOWN_ID = 6;

    public static Direction NONE = new Direction(Pos.zero, Vector3.zero, "NONE", EnumAxis.NONE, NONE_ID, NONE_ID, NONE_ID, NONE_ID, 0);
    /// <summary> +Z </summary>
    public static Direction NORTH = new Direction(Pos.north, Vector3.forward, "North", EnumAxis.Z, SOUTH_ID, EAST_ID, WEST_ID, NORTH_ID, N_MASK);
    /// <summary> +X </summary>
    public static Direction EAST = new Direction(Pos.east, Vector3.right, "East", EnumAxis.X, WEST_ID, SOUTH_ID, NORTH_ID, EAST_ID, E_MASK);
    /// <summary> -Z </summary>
    public static Direction SOUTH = new Direction(Pos.south, Vector3.back, "South", EnumAxis.Z, NORTH_ID, WEST_ID, EAST_ID, SOUTH_ID, S_MASK);
    /// <summary> -X </summary>
    public static Direction WEST = new Direction(Pos.west, Vector3.left, "West", EnumAxis.X, EAST_ID, NORTH_ID, SOUTH_ID, WEST_ID, W_MASK);
    /// <summary> +Y </summary>
    public static Direction UP = new Direction(Pos.up, Vector3.up, "Up", EnumAxis.Y, DOWN_ID, UP_ID, UP_ID, UP_ID, U_MASK);
    /// <summary> -Y </summary>
    public static Direction DOWN = new Direction(Pos.down, Vector3.down, "Down", EnumAxis.Y, UP_ID, DOWN_ID, DOWN_ID, DOWN_ID, D_MASK);

    /// <summary> North, East, South, West </summary>
    public static Direction[] horizontal = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
    /// <summary> North, East, South, West, Up, Down </summary>
    public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };
    /// <summary> None, North, East, South, West, Up, Down </summary>
    public static Direction[] allIncludeNone = new Direction[] { Direction.NONE, Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };

    /// <summary> A Pos pointing in this direction. </summary>
    public readonly Pos pos;
    /// <summary> A Vector3 pointing in this direction. </summary>
    public readonly Vector3 vector;
    public readonly EnumAxis axis;
    public readonly int index;
    public readonly int directionMask;

    private string name;
    private int oppositeIndex;
    private int clockwiseIndex;
    private int counterClockwiseIndex;

    private Direction(Pos pos, Vector3 vec, string name, EnumAxis axis, int opposite, int clockwise, int counterClockwise, int directionIndex, int directionMask) {
        this.pos = pos;
        this.vector = vec;
        this.name = name;
        this.axis = axis;
        this.oppositeIndex = opposite;
        this.clockwiseIndex = clockwise;
        this.counterClockwiseIndex = counterClockwise;
        this.index = directionIndex;
        this.directionMask = directionMask;
    }

    public Direction getOpposite() {
        return Direction.allIncludeNone[this.oppositeIndex];
    }

    /// <summary>
    /// Gets the direction that is clockwise of this, rotating on the y axis.  Returns itself for SELF, UP, DOWN.
    /// </summary>
    public Direction getClockwise() {
        return Direction.allIncludeNone[this.clockwiseIndex];
    }

    /// <summary>
    /// Gets the direction that is counter clockwise of this, rotating on the y axis.  Returns itself for SELF, UP, DOWN.
    /// </summary>
    public Direction getCounterClockwise() {
        return Direction.allIncludeNone[this.counterClockwiseIndex];
    }

    public override string ToString() {
        return this.name;
    }
}
