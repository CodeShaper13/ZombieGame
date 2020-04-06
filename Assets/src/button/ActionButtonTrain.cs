public class ActionButtonTrain : ActionButtonChild {

    private readonly string buttonText;
    private readonly UnitData unitData;

    public ActionButtonTrain(RegisteredObject obj) : base(string.Empty) {
        this.unitData = obj.getPrefab().GetComponent<UnitBase>().unitData;
        this.buttonText = this.unitData.unitName + " (" + 0 + ")";

        this.setMainActionFunction((unit) => {
            BuildingTrainingHouse trainingHouse = (BuildingTrainingHouse)unit;
            if(trainingHouse.tryAddToQueue(obj)) {
                // Remove resources
                trainingHouse.map.reduceResources(trainingHouse.getTeam(), 0);
            }
        });

        this.setShouldDisableFunction((entity) => {
            BuildingTrainingHouse trainingHouse = ((BuildingTrainingHouse)entity);
            return 0 > Player.localPlayer.currentTeamResources || trainingHouse.isQueueFull();
        });
    }

    public override string getText() {
        return buttonText;
    }
}