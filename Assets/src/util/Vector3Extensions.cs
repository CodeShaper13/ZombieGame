using UnityEngine;

public static class Vector3Extensions {

    public static Vector3 setX(this Vector3 vector, float f) {
        return new Vector3(f, vector.y, vector.z);
    }

    public static Vector3 setY(this Vector3 vector, float f) {
        return new Vector3(vector.x, f, vector.z);
    }

    public static Vector3 setZ(this Vector3 vector, float f) {
        return new Vector3(vector.x, vector.y, f);
    }

    /// <summary>
    /// Converts the Vector into a 2 dimension Vector by removing the y value.
    /// </summary>
    public static Vector2 toVector2(this Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }
}
