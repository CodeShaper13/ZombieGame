namespace codeshaper.entity.unit.task.builder {

    public class TaskConstructBuilding : TaskRepair {

        public TaskConstructBuilding(UnitBuilder unit, BuildingBase newBuilding) : base(unit, newBuilding, true) {
            this.building.setConstructing();

            this.unit.unitStats.buildingsBuilt.increase();
        }

        public override bool cancelable() {
            return false;
        }

        protected override bool shouldContinue() {
            return this.building.isConstructing();
        }
    }
}
