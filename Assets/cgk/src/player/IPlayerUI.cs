using UnityEngine;

public interface IPlayerUI {

    /// <summary>
    /// Returns this Player's transfrom.
    /// </summary>
    Transform getTransform();

    void setPlayer(IPlayer owner);
}
