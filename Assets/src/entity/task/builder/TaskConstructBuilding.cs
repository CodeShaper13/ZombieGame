public class TaskConstructBuilding : TaskRepair {

    public TaskConstructBuilding(UnitBuilder unit) : base(unit) { }

    public TaskConstructBuilding(UnitBuilder unit, BuildingBase newBuilding) : base(unit, newBuilding) {
        this.building.setConstructing();
    }

    protected override bool shouldContinue() {
        return this.building.isConstructing();
    }
}