using System.Collections.Generic;

public class GameModeElimination : GameModeBase {

    private List<MapObject> enemyObjs;

    public override void init(MapBase map) {
        base.init(map);

        this.enemyObjs = new List<MapObject>(this.map.findMapObjects(Team.ZOMBIES.predicateThisTeam));
    }

    public override void updateGameMode() {
        base.updateGameMode();

        this.enemyObjs.RemoveAll(item => item is LivingObject && !Util.isAlive((LivingObject)item));
        if(this.enemyObjs.Count <= 0) {
            this.triggerLoss();
        }
    }

    protected override string getDescription() {
        throw new System.NotImplementedException();
    }

    protected override string getGameModeName() {
        return "Elimination";
    }
}
