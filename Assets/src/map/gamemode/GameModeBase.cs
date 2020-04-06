using UnityEngine;

public abstract class GameModeBase : MonoBehaviour {

    protected MapBase map;

    public virtual void init(MapBase map) {
        this.map = map;
    }

    public virtual void updateGameMode() { }

    /// <summary>
    /// Call to trigger a win of the game mode.
    /// </summary>
    protected void triggerWin() {
        //TODO
    }

    /// <summary>
    /// Call to triger a loss of the game mode.
    /// </summary>
    protected void triggerLoss() {
        //TODO
    }

    protected abstract string getGameModeName();

    protected abstract string getDescription();
}
