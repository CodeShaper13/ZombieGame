using UnityEngine;
using System.Collections;

public struct Pos {
    /// <summary> Short for 0, 0, 0 </summary>
    public static Pos zero = new Pos(0, 0, 0);
    /// <summary> Short for 1, 1, 1 </summary>
    public static Pos one = new Pos(1, 1, 1);
    /// <summary> Short for 0, 0, 1 </summary>
    public static Pos north = new Pos(0, 0, 1);
    /// <summary> Short for 1, 0, 0 </summary>
    public static Pos east = new Pos(1, 0, 0);
    /// <summary> Short for 0, 0, -1 </summary>
    public static Pos south = new Pos(0, 0, -1);
    /// <summary> Short for -1, 0, 0 </summary>
    public static Pos west = new Pos(-1, 0, 0);
    /// <summary> Short for 0, 1, 0 </summary>
    public static Pos up = new Pos(0, 1, 0);
    /// <summary> Short for 0, -1, 0 </summary>
    public static Pos down = new Pos(0, -1, 0);

    public int x;
    public int y;
    public int z;

    public Pos(int i) {
        this.x = i;
        this.y = i;
        this.z = i;
    }

    public Pos(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Pos(Vector3 vector) {
        this.x = Mathf.RoundToInt(vector.x);
        this.y = Mathf.RoundToInt(vector.y);
        this.z = Mathf.RoundToInt(vector.z);
    }

    /// <summary>
    /// Returns a new block pos moved in the passes direction.
    /// </summary>
    public Pos move(Direction direction) {
        return new Pos(this.x + direction.pos.x, this.y + direction.pos.y, this.z + direction.pos.z);
    }

    public override string ToString() {
        return "(" + this.x + ", " + this.y + ", " + this.z + ")";
    }

    public override bool Equals(object obj) {
        return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode() {
        unchecked {
            int hash = 47;
            hash = hash * 227 + x;
            hash = hash * 227 + y;
            hash = hash * 227 + z;
            return hash;
        }
    }

    /// <summary>
    /// Adds the passed values to this pos.
    /// </summary>
    public Pos add(int x, int y, int z) {
        return new Pos(this.x + x, this.y + y, this.z + z);
    }

    /// <summary>
    /// Adds the passed values to this pos.
    /// </summary>
    public Pos subtract(int x, int y, int z) {
        return new Pos(this.x - x, this.y - y, this.z - z);
    }


    public static Pos operator +(Pos b, Pos b1) {
        return new Pos(b.x + b1.x, b.y + b1.y, b.z + b1.z);
    }

    public static Pos operator -(Pos b, Pos b1) {
        return new Pos(b.x - b1.x, b.y - b1.y, b.z - b1.z);
    }

    public static Pos operator *(Pos b, int i) {
        return new Pos(b.x * i, b.y * i, b.z * i);
    }

    public static Pos operator /(Pos b, int i) {
        return new Pos(b.x / i, b.y / i, b.z / i);
    }

    /// <summary>
    /// Adds 1 to all of the axes that are not 0.
    /// </summary>
    public static Pos operator ++(Pos p) {
        return new Pos(
            p.x + (p.x != 0 ? 1 : 0),
            p.y + (p.y != 0 ? 1 : 0),
            p.z + (p.z != 0 ? 1 : 0));
    }

    /// <summary>
    /// Subtracts 1 from all of the axes that are not 0.
    /// </summary>
    public static Pos operator --(Pos p) {
        return new Pos(
            p.x - (p.x != 0 ? 1 : 0),
            p.y - (p.y != 0 ? 1 : 0),
            p.z - (p.z != 0 ? 1 : 0));
    }

    /// <summary>
    /// Coverts the BlockPos to a Vector3
    /// </summary>
    public Vector3 toVector() {
        return new Vector3(this.x, this.y, this.z);
    }

    /// <summary>
    /// Rotates a BlockPos around a pivot and returns it.
    /// </summary>
    public Pos rotateAround(Pos pivot, Quaternion angle) { // TODO optimize!
        return new Pos(this.toVector().rotateAround(pivot.toVector(), angle));
    }

    public static Pos fromRaycastHit(RaycastHit hit) {
        Vector3 vec = hit.point + ((hit.normal * -1f) / 100);
        return new Pos(vec);
        /*
        float f = adjacent ? 1f : -1f;
        BlockPos pos = BlockPos.fromVec(new Vector3(
            BlockPos.moveWithinBlock(hit.point.x + (hit.normal.x / 4) * f, hit.normal.x, adjacent),
            BlockPos.moveWithinBlock(hit.point.y + (hit.normal.y / 4) * f, hit.normal.y, adjacent),
            BlockPos.moveWithinBlock(hit.point.z + (hit.normal.z / 4) * f, hit.normal.z, adjacent)));
        return pos;
        */
    }

    /// <summary>
    /// Returns true if the pos's x, y, and z are all 0.
    /// </summary>
    public bool isZero() {
        return this.x == 0 && this.y == 0 && this.z == 0;
    }

    public Direction toDirection() {
        if(this.Equals(Pos.north)) {
            return Direction.NORTH;
        }
        else if(this.Equals(Pos.east)) {
            return Direction.EAST;
        }
        else if(this.Equals(Pos.west)) {
            return Direction.WEST;
        }
        else if(this.Equals(Pos.south)) {
            return Direction.SOUTH;
        }
        else if(this.Equals(Pos.up)) {
            return Direction.UP;
        }
        else if(this.Equals(Pos.down)) {
            return Direction.DOWN;
        }
        return Direction.NONE;
    }
}
