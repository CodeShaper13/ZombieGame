using fNbt;
using UnityEngine;

public class TaskRepair : TaskBase<UnitBuilder> {

    private const float TIME_SPENT_WHACKING = 1.5f;

    protected BuildingBase building;

    /// <summary> How much time the builder has left at his current point. </summary>
    private float timeWhacking = TIME_SPENT_WHACKING;
    /// <summary> The current point that the builder is walking to/at. </summary>
    private Vector3 whackPoint;
    /// <summary> Points around the Building that the Builder goes to while whacking it. </summary>
    private Vector3[] points;

    public TaskRepair(UnitBuilder unit) : base(unit) { }

    public TaskRepair(UnitBuilder unit, BuildingBase newBuilding) : this(unit) {
        this.setBuilding(newBuilding);

        this.whackPoint = this.pickWhackPoint();
        this.moveHelper.setDestination(this.whackPoint);
    }

    private void setBuilding(BuildingBase newBuilding) {
        this.building = newBuilding;

        // Generates the 4 whack points that the Builder goes to.
        Vector3 bPos = this.building.getPos();
        Vector2 footprint = this.building.getFootprintSize() / 2;
        const float f = 0.5f;
        this.points = new Vector3[] {
                bPos + new Vector3(footprint.x + f, 0, 0),
                bPos + new Vector3(0, 0, footprint.y + f),
                bPos + new Vector3(-footprint.x - f, 0, 0),
                bPos + new Vector3(0, 0, -footprint.y - f) };
    }

    public override bool preform(float deltaTime) {
        if(Util.isAlive(this.building)) {
            if(this.isNextToWhackPoint()) {
                this.moveHelper.stop();
                this.building.increaseConstructed(true);

                this.timeWhacking -= deltaTime;
                if(this.timeWhacking <= 0) {
                    // Pick a new point to go to.
                    this.whackPoint = this.pickWhackPoint();
                    this.timeWhacking = TIME_SPENT_WHACKING;
                }
            }
            else {
                this.moveHelper.setDestination(this.whackPoint);
            }
            return this.shouldContinue();
        }
        else {
            return false; // End task, building was destroyed/is gone.
        }
    }

    public override void drawDebug() {
        foreach(Vector3 point in this.points) {
            Color color = point == this.whackPoint ? Colors.orange : Color.yellow;
            GLDebug.DrawArrow(point, Vector3.up * 2.5f, 0.25f, 20, color);
        }

        if(Util.isAlive(this.building)) {
            GLDebug.DrawLine(this.unit.getFootPos(), this.building.getPos(), this.isNextToWhackPoint() ? Colors.green : Colors.red);
        }
    }

    protected virtual bool shouldContinue() {
        if(this.building.getHealth() == this.building.getMaxHealth()) {
            this.unit.unitStats.repairsDone.increase();
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if the builder is next to a whacking point and should stop moving and get to work.
    /// </summary>
    private bool isNextToWhackPoint() {
        return this.getDistance(this.whackPoint) < 0.25f;
    }

    /// <summary>
    /// Picks a random point for the builder to whack.
    /// </summary>
    private Vector3 pickWhackPoint() {
        return this.points[Random.Range(0, this.points.Length - 1)];
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.timeWhacking = tag.getFloat("timeWhacking");
        this.whackPoint = tag.getVector3("whackPoint");
        this.moveHelper.setDestination(this.whackPoint);
        BuildingBase b = this.unit.map.findMapObjectFromGuid<BuildingBase>(tag.getGuid("building"));
        if(b != null) {
            this.setBuilding(b);
        }
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        if(this.building != null) {
            tag.setTag("building", this.building.getGuid());
        }
        tag.setTag("timeWhacking", this.timeWhacking);
        tag.setTag("whackPoint", this.whackPoint);
    }
}