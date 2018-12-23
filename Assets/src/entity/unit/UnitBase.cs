using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class UnitBase : SidedEntity {

    public MoveHelper moveHelper;
    private ITask task;
    public AttackBase attack;

    private Vector3? overrideMovementDestination;
    private float overrideMovementStopDis;

    public UnitStats unitStats;
    /// <summary> The position of the unit during the last frame. </summary>
    private Vector3 lastPos;

    public override void onAwake() {
        base.onAwake();

        this.moveHelper = new MoveHelper(this);
        this.attack = this.createAttackMethod();
    }

    public override void onStart() {
        base.onStart();

        this.unitStats = new UnitStats(this.getData());
        this.setTask(null);
    }

    public override void onUpdate(float deltaTime) {
        base.onUpdate(deltaTime);

        if(this.overrideMovementDestination != null) {
            if(Vector3.Distance(this.getFootPos(), (Vector3)this.overrideMovementDestination) <= this.overrideMovementStopDis + 0.5f) {
                this.overrideMovementDestination = null;
            }
        }
        else if(this.task != null) {
            bool continueExecuting = this.task.preform();
            if(!continueExecuting) {
                this.setTask(null, true); // Set unit to idle.
            }
        }

        // Update stats.
        if(this.transform.position != this.lastPos) {
            this.unitStats.distanceWalked.increase(Vector3.Distance(this.transform.position, this.lastPos));
        }
        this.lastPos = this.transform.position;

        this.unitStats.timeAlive.increase(Time.deltaTime);
    }

    public override void drawDebug() {
        base.drawDebug();

        // Draw a debug arrow pointing forward.
        GLDebug.DrawLineArrow(this.getPos(), this.getPos() + this.transform.forward, 0.5f, 20, Color.blue, 0, true);
        this.moveHelper.drawDebug();

        if(this.overrideMovementDestination == null && this.task != null) {
            this.task.drawDebug();
        }
    }

    public override void colorObject() {
        base.colorObject();

        Color color = this.getTeam().getTeamColor();
        this.GetComponent<MeshRenderer>().material.color = color;
    }

    public virtual Vector3 getFootPos() {
        return this.transform.position - new Vector3(0, -1, 0);
    }

    public override float getSizeRadius() {
        return 0.5f;
    }

    public abstract EntityBaseStats getData();

    /// <summary>
    /// Sets the units task.  Pass null to set the current task to idle.
    /// </summary>
    public void setTask(ITask newTask, bool forceCancelPrevious = false) {
        // Explicitly setting a task while a unit is moving stops it's walk.
        this.overrideMovementDestination = null;

        if(this.task == null || (forceCancelPrevious || (this.task != null && this.task.cancelable()))) {
            // If there is an old task, call finish on the instance.
            if(this.task != null) {
                this.task.onFinish();
            }

            this.task = (newTask == null) ? new TaskAttackNearby(this) : newTask;
            //this.task = (newTask == null) ? new TaskIdle(this) : newTask;
        }
    }

    /// <summary>
    /// Returns the Unit's current task.  This will never be null.
    /// </summary>
    public ITask getTask() {
        return this.task;
    }

    /// <summary>
    /// Moves a unit to a certain point.  This overrides their current task as long as it is cancelable.
    /// </summary>
    public void walkToPoint(Vector3 point, int partySize) {
        if(this.task.cancelable()) {
            this.overrideMovementStopDis = partySize <= 1 ? 0f : (partySize <= 3 ? 1f : 3f); // TODO spread out stopping points.
            this.overrideMovementDestination = point;

            // Move the units to the destination, making sure they don't pile to close.
            this.moveHelper.setDestination(point, this.overrideMovementStopDis);
        }
    }

    public virtual AttackBase createAttackMethod() {
        return new AttackMelee(this);
    }

    public abstract int getAttackAmount();

    public override float getHealthBarHeight() {
        return 1.95f;
    }

    /// <summary>
    /// Damages the passed object and returns it.  Null will be returned if the object is destroyed.
    /// This method will also increase stats if needed.
    /// </summary>
    public SidedEntity damageTarget(SidedEntity obj) {
        int damage = this.getAttackAmount();
        this.unitStats.damageDelt.increase(damage);

        if(obj.damage(this, damage)) {
            // obj was killed.
            if(obj is BuildingBase) {
                this.unitStats.buildingsDestroyed.increase();
            }
            else if(obj is UnitBase) {
                this.unitStats.unitsKilled.increase();
            }
            return null;
        }
        else {
            return obj;
        }
    }
}
