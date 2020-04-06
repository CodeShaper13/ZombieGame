using UnityEngine;

public interface IPlayer {

    /// <summary>
    /// Returns this Player's transfrom.
    /// </summary>
    Transform getTransform();

    /// <summary>
    /// Returns the Camera this player is using.
    /// Multiple Players can return the same Camera for a shared screen situation.
    /// </summary>
    Camera getCamera();
}
