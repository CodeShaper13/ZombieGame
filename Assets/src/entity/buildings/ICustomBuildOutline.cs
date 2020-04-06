using UnityEngine;

public interface ICustomBuildOutline {

    /// <summary>
    /// True is returned the modifications were made, false if they were not and the default square should be shown.
    /// </summary>
    bool func(Vector3 point, Transform outlineTransform);
}
