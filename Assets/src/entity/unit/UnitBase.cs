using fNbt;
using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NavAgent2D))]
public abstract class UnitBase : SidedEntity {

    // Set via inspector
    public UnitData unitData;

    public MoveHelper moveHelper;

    private ITask task;
    [SyncVar]
    private bool taskCancelable;

    public AttackBase attack;

    private Vector3? overrideMovementDestination;
    private float overrideMovementStopDis;

    public UnitStats unitStats;
    /// <summary> The position of the unit during the last frame. </summary>
    private Vector3 lastPos;

    public override void onServerAwake() {
        base.onServerAwake();

        this.attack = this.createAttackMethod();
        this.moveHelper = new MoveHelper(this);
    }

    public override void OnStartServer() {
        base.OnStartServer();

        this.unitStats = new UnitStats(this.unitData);
        this.setTask(null);
    }

    public string debug;

    public override void onUpdate(float deltaTime) {
        base.onUpdate(deltaTime);

        this.debug = this.getTask().ToString();

        if(this.overrideMovementDestination != null) {
            if(Vector3.Distance(this.getFootPos(), (Vector3)this.overrideMovementDestination) <= this.overrideMovementStopDis + 0.5f) {
                this.overrideMovementDestination = null;
            }
        }
        else {
            bool continueExecuting = this.task.preform(deltaTime);
            if(!continueExecuting) {
                print("setitng to idle");
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

    [ClientSideOnly]
    public override void drawDebug() {
        base.drawDebug();

        // Draw a debug arrow pointing forward.
        //GLDebug.DrawLineArrow(this.getPos(), this.getPos() + this.transform.forward, 0.5f, 20, Color.blue, 0, true);

        if(this.isServer) {
            //this.moveHelper.drawDebug();

            if(this.overrideMovementDestination == null && this.task != null) {
                this.task.drawDebug();
            }
        }
    }

    public override int getMaxHealth() {
        return this.unitData.maxHealth;
    }

    public override void colorObject() {
        base.colorObject();

        Color color = this.getTeam().getColor();
//        this.GetComponent<MeshRenderer>().material.color = color;
    }

    /// <summary>
    /// Returns the position of this Units feet.
    /// </summary>
    public virtual Vector3 getFootPos() {
        return this.transform.position - new Vector3(0, 0.5f, 0);
    }

    public override float getSizeRadius() {
        return 0.5f;
    }

    public override string getDisplayName() {
        return this.unitData.unitName;
    }

    public override int getButtonMask() {
        return base.getButtonMask() | ActionButton.unitIdle;
    }

    /// <summary>
    /// Sets the units task.  Pass null to set the current task to Idle (default task).
    /// </summary>
    [ServerSideOnly]
    public void setTask(ITask newTask, bool forceCancelPrevious = false) {
        // Explicitly setting a task while a unit is moving stops it's walk.
        this.overrideMovementDestination = null;

        if(this.task == null || (forceCancelPrevious || (this.task != null && this.isTaskCancelable()))) {
            // If there is an old task, call finish on the instance.
            if(this.task != null) {
                this.task.onFinish();
            }

            this.task = (newTask == null) ? new TaskIdle(this) : newTask;
            this.taskCancelable = this.task is ICancelableTask;
        }
    }

    /// <summary>
    /// Returns the Unit's current task.  This will never be null.
    /// </summary>
    [ServerSideOnly]
    public ITask getTask() {
        return this.task;
    }

    /// <summary>
    /// Returns true if the Unit's task can be canceled.
    /// </summary>
    public bool isTaskCancelable() {
        return this.taskCancelable;
    }

    /// <summary>
    /// Moves a unit to a certain point.  This overrides their current task as long as it is cancelable.
    /// </summary>
    [ServerSideOnly]
    public void walkToPoint(Vector3 point, int partySize) {
        if(this.isTaskCancelable()) {
            float f;
            if(partySize <= 1) {
                f = 0.1f;
            } else if(partySize <= 2) {
                f = 1f;
            }
            else if(partySize <= 4) {
                f = 2f;
            }
            else if(partySize <= 8) {
                f = 3f;
            }
            else if(partySize <= 16) {
                f = 4f;
            } else {
                f = 8f;
            }
            this.overrideMovementStopDis = f;
            this.overrideMovementDestination = point;

            // Move the units to the destination, making sure they don't pile to close.
            this.moveHelper.setDestination(point, this.overrideMovementStopDis);
        }
    }

    public virtual AttackBase createAttackMethod() {
        return new AttackMelee(this);
    }

    public int getAttackAmount() {
        return this.unitData.attack;
    }

    public override float getHealthBarHeight() {
        return 0.7f;
    }

    [ServerSideOnly]
    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.lastPos = tag.getVector3("lastPos");
        this.unitStats = new UnitStats(tag, this.unitData);

        bool flag = tag.getBool("hasMovementOverride");
        if(flag) {
            this.overrideMovementDestination = tag.getVector3("overrideMovementDestination");
            this.overrideMovementStopDis = tag.getFloat("overrideMovementStopDis");
            this.moveHelper.setDestination((Vector3)this.overrideMovementDestination, this.overrideMovementStopDis);
        }

        NbtCompound tagTask = tag.getCompound("task");
        ITask task = (ITask)Activator.CreateInstance(TaskManager.getTaskFromId(tagTask.getInt("taskID")), new object[] { (UnitBase)this });
        task.readFromNbt(tagTask);
        this.setTask(task, true);
    }

    [ServerSideOnly]
    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("lastPos", this.lastPos);
        this.unitStats.writeToNBT(tag);

        tag.setTag("hasMovementOverride", this.overrideMovementDestination != null);
        if(this.overrideMovementDestination != null) {
            tag.setTag("overrideMovementDestination", (Vector3)this.overrideMovementDestination);
            tag.setTag("overrideMovementStopDis", this.overrideMovementStopDis);
        }

        NbtCompound taskTag = new NbtCompound("task");
        taskTag.setTag("taskID", TaskManager.getIdFromTask(this.task));
        task.writeToNbt(taskTag);
        tag.Add(taskTag);
    }

    /// <summary>
    /// Damages the passed object and returns it.  Null will be returned if the object is destroyed.
    /// This method will also increase stats if needed.
    /// </summary>
    [ServerSideOnly]
    public LivingObject damageTarget(LivingObject obj) {
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
