using UnityEngine;

public class GameModeRepair : GameModeBase {

    [SerializeField]
    private BuildingBase target;

    public override void updateGameMode() {
        base.updateGameMode();

        if(target.getHealth() >= target.getMaxHealth()) {
            this.triggerWin();
        }
    }

    protected override string getDescription() {
        throw new System.NotImplementedException();
    }

    protected override string getGameModeName() {
        return "Repair";
    }
}
