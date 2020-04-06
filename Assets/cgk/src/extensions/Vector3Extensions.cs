using System;
using UnityEngine;

public static class Vector3Extensions {

    public static Vector3 setX(this Vector3 vector, float x) {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 setY(this Vector3 vector, float y) {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 setZ(this Vector3 vector, float z) {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector3 round(this Vector3 vector, int places) {
        return new Vector3((float)Math.Round(vector.x, places), (float)Math.Round(vector.y, places), (float)Math.Round(vector.z, places));
    }

    public static Vector3 addX(this Vector3 vector, float f) {
        return new Vector3(vector.x + f, vector.y, vector.z);
    }

    public static Vector3 addY(this Vector3 vector, float f) {
        return new Vector3(vector.x, vector.y + f, vector.z);
    }

    public static Vector3 addZ(this Vector3 vector, float f) {
        return new Vector3(vector.x, vector.y, vector.z + f);
    }

    /// <summary>
    /// Rotates the Vector around a pivot and returns it.
    /// </summary>
    public static Vector3 rotateAround(this Vector3 vector, Vector3 pivot, Quaternion angle) {
        if(angle == new Quaternion(0, 0, 0, 1)) {
            return vector;
        }
        return angle * (vector - pivot) + pivot;
    }

    /// <summary>
    /// Returns a vector with the absolute values of the passed Vector3.
    /// </summary>
    public static Vector3 abs(this Vector3 vector) {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    /// <summary>
    /// Converts the Vector into a 2 dimension Vector by removing the y value.
    /// </summary>
    public static Vector2 toVector2CutY(this Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }
}
