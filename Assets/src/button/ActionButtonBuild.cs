public class ActionButtonBuild : ActionButtonChild {

    private readonly string buttonText;
    private BuildingData buildingData;

    public ActionButtonBuild(string actionName, RegisteredObject obj) : base(actionName) {
        this.setMainActionFunction((unit) => {
            Player.localPlayer.enableBuildOutline(obj, (UnitBuilder)unit);
        });
        this.setExecuteOnClientSide();

        this.buildingData = obj.getPrefab().GetComponent<BuildingBase>().getData();
        this.buttonText = this.buildingData.getName() + " (" + this.buildingData.getCost() + ")";

        this.setShouldDisableFunction((entity) => {
            int cost = this.buildingData.getCost();
            return cost > Player.localPlayer.currentTeamResources;
            //return cost > entity.getTeam().getResources();
        });
    }

    public override string getText() {
        return buttonText;
    }
}
