using UnityEngine;

public class TaskRepair : TaskBase<UnitBuilder> {

    private const float F_01 = 3f;

    protected BuildingBase building;
    protected bool isConstructing;

    private float timeWhacking = F_01;
    private Vector3 whackPoint;
    private Vector3[] points;

    public TaskRepair(UnitBuilder unit, BuildingBase newBuilding) : this(unit, newBuilding, false) { }

    protected TaskRepair(UnitBuilder unit, BuildingBase newBuilding, bool isConstructing) : base(unit) {
        this.building = newBuilding;
        this.isConstructing = isConstructing;

        Vector3 bPos = this.building.getPos();
        Vector2 footprint = this.building.getFootprintSize() / 2;
        const float f = 0.5f;
        this.points = new Vector3[] {
                bPos + new Vector3(footprint.x + f, 0, 0),
                bPos + new Vector3(0, 0, footprint.y + f),
                bPos + new Vector3(-footprint.x - f, 0, 0),
                bPos + new Vector3(0, 0, -footprint.y - f) };

        this.whackPoint = this.pickWhackPoint();

        this.moveHelper.setDestination(this.whackPoint);
    }

    public override bool preform() {
        if(Util.isAlive(this.building)) {
            if(this.isNextToWhackPoint()) {
                this.moveHelper.stop();
                this.building.increaseConstructed(true);

                this.timeWhacking -= Time.deltaTime;
                if(this.timeWhacking <= 0) {
                    // Pick a new point to go to.
                    this.whackPoint = this.pickWhackPoint();
                    this.timeWhacking = F_01;
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

    private bool isNextToWhackPoint() {
        return this.getDistance(this.whackPoint) < 0.25f;
    }

    /// <summary>
    /// Picks a random point for the builder to whack.
    /// </summary>
    private Vector3 pickWhackPoint() {
        return this.points[Random.Range(0, this.points.Length - 1)];
    }
}