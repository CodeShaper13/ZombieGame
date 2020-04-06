using UnityEngine;

public class GameModeDTP : GameModeTimer {

    [SerializeField]
    private BuildingBase building;

    public override void updateGameMode() {
        base.updateGameMode();

        if(!Util.isAlive(this.building)) {
            this.triggerLoss();
        }
    }

    protected override string getDescription() {
        throw new System.NotImplementedException();
    }

    protected override string getGameModeName() {
        return "Defend the Point";
    }
}
