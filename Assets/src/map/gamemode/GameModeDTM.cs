using UnityEngine;

public class GameModeDTM : GameModeBase {

    [SerializeField]
    private BuildingBase monument;

    public override void updateGameMode() {
        base.updateGameMode();

        if(!Util.isAlive(this.monument)) {
            this.triggerWin();
        }
    }

    protected override string getDescription() {
        throw new System.NotImplementedException();
    }

    protected override string getGameModeName() {
        return "Destroy the Monument";
    }
}
