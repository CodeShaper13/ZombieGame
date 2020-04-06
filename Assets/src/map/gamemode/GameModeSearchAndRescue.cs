using UnityEngine;

public class GameModeSearchAndRescue : GameModeBase {

    [SerializeField]
    private UnitBase target;
    [Tooltip("The location the target should be brought to.")]
    public Bounds destination = new Bounds(Vector3.zero, Vector3.one * 2);

    public override void updateGameMode() {
        base.updateGameMode();

        // Check if the target has died.
        if(!Util.isAlive(this.target)) {
            this.triggerLoss();
        }

        // Check if the target has made it to the destination.
        if(this.destination.Contains(this.target.getPos())) {
            this.triggerWin();
        }
    }

    protected override string getGameModeName() {
        throw new System.NotImplementedException();
    }

    protected override string getDescription() {
        return "Search and Rescue";
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Colors.lightBlue;
        Gizmos.DrawWireCube(this.destination.center, this.destination.size);
    }
}
