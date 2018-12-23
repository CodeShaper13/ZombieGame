public class ActionButtonTrain : ActionButtonChild {

    private readonly string buttonText;
    private readonly EntityBaseStats entityData;

    public ActionButtonTrain(RegisteredObject obj) : base(string.Empty) {
        this.entityData = obj.getPrefab().GetComponent<UnitBase>().getData();
        this.buttonText = this.entityData.getUnitTypeName() + " (" + this.entityData.cost + ")";

        this.setMainActionFunction((unit) => {
            BuildingTrainingHouse trainingHouse = (BuildingTrainingHouse)unit;
            if(trainingHouse.tryAddToQueue(obj)) {
                // Remove resources
                trainingHouse.getTeam().reduceResources(this.entityData.cost);
            }
        });

        this.setShouldDisableFunction((entity) => {
            return this.entityData.cost > entity.getTeam().getResources();
        });
    }

    public override string getText() {
        return buttonText;
    }
}