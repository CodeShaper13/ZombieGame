using UnityEngine;

public abstract class BuildingBase : SidedEntity {

    /// <summary>
    /// True if the building is being constructed.  Buildings that
    /// are still being built can not function.
    /// </summary>
    private bool constructing;
    private float buildProgress;

    private int targetRotation = 0;

    public override void onUpdate(float deltaTime) {
        if(!this.constructing) {
            this.preformTask(deltaTime);
        }

        // Rotate the building slowly if it is being rotated.
        if(this.transform.eulerAngles.y != this.targetRotation) {
            this.transform.rotation = Quaternion.RotateTowards(
                this.transform.rotation,
                Quaternion.Euler(0, this.targetRotation, 0),
                250 * deltaTime);
        }
    }

    public override void drawDebug() {
        base.drawDebug();

        Vector2 v = this.getFootprintSize();
        GLDebug.DrawCube(this.transform.position, Quaternion.identity, new Vector3(v.x, 0.35f, v.y), Colors.purple);
    }

    public override int getButtonMask() {
        int mask = base.getButtonMask();
        if(!(this is BuildingWall)) {
            mask |= ActionButton.buildingRotate.getMask();
        }
        return mask;
    }

    /// <summary>
    /// Sets the building to be currently being constructed by a builder.
    /// </summary>
    public void setConstructing() {
        this.constructing = true;
        this.buildProgress = 1;
    }

    public bool isConstructing() {
        return this.constructing;
    }

    public override float getSizeRadius() {
        Vector2 v = this.getFootprintSize();
        return (Mathf.Max(v.x, v.y) / 2);
    }

    /// <summary>
    /// Called every frame for the building to preform it's task, if it has any.
    /// </summary>
    protected virtual void preformTask(float deltaTime) { }

    /// <summary>
    /// Returns the size of the building as a Vector2 of (width, height).
    /// </summary>
    public abstract Vector2 getFootprintSize();

    public abstract BuildingData getData();

    public override int getMaxHealth() {
        return this.getData().getMaxHealth();
    }

    /// <summary>
    /// Used to continue to construct a building or to repair it.
    /// Returns true if the building was finished on this call.
    /// </summary>
    public bool increaseConstructed(bool deductResources) {
        this.buildProgress += (Constants.CONSTRUCT_RATE * Time.deltaTime);
        this.setHealth((int)this.buildProgress);

        if(deductResources && (int)buildProgress > this.getHealth()) {
            this.getTeam().reduceResources(1);
        }

        if((int)this.buildProgress >= this.getMaxHealth()) {
            this.constructing = false;
            this.buildProgress = 0;
            return true;
        }

        return false;
    }

    public int getCost() {
        return this.getData().getCost();
    }

    public void rotateBuilding() {
        this.targetRotation += 90;
        if(this.targetRotation >= 360) {
            this.targetRotation = 0;
        }
    }
}
