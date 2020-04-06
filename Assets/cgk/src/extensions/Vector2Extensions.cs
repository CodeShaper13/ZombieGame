using UnityEngine;

public static class Vector2Extensions {

    /// <summary>
    /// Converts the Vector2 into a Vector2Int by flooring the numbers.
    /// </summary>
    public static Vector2Int floor(this Vector2 vector) {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }

    /// <summary>
    /// Converts the Vector2 into a Vector2Int by ceiling the numbers.
    /// </summary>
    public static Vector2Int ceil(this Vector2 vector) {
        return new Vector2Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y));
    }

    /// <summary>
    /// Converts the Vector2 into a Vector2Int by ceiling the numbers.
    /// </summary>
    public static Vector2Int toInt(this Vector2 vector) {
        return new Vector2Int((int)vector.x, (int)vector.y);
    }
}
