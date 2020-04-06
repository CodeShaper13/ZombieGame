using UnityEngine;

public interface IRandomGenerator {

    /// <summary>
    /// Returns a random rotation on the y axis.
    /// </summary>
    Quaternion rndYRot();

    /// <summary>
    /// Returns a random integer between min (inclusive) and max (exclusive).
    /// </summary>
    int rndInt(int min, int max);

    /// <summary>
    /// Returns a random integer between min (inclusive) and max (exclusive).
    /// </summary>
    float rndFloat(float min, float max);

    /// <summary>
    /// Returns a random boolean.
    /// </summary>
    /// <returns></returns>
    bool rndBool();
}
