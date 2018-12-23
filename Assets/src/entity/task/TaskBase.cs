using System;
using UnityEngine;

public abstract class TaskBase<T> : ITask where T : UnitBase {

    /// <summary>
    /// The Unit running the task.
    /// </summary>
    protected readonly T unit;
    protected readonly MoveHelper moveHelper;

    public TaskBase(T unit) {
        this.unit = unit;
        moveHelper = this.unit.moveHelper;
    }

    /// <summary>
    /// Returns the closest enemy object to this unit, or null if there are none in range.
    /// </summary>
    protected T findEntityOfType<T>(float maxDistance) where T : SidedEntity {
        return findEntityOfType<T>(unit.getFootPos(), maxDistance);
    }

    /// <summary>
    /// Returns the closest enemy object to the point, or null if there are none.
    /// </summary>
    protected T findEntityOfType<T>(Vector3 point, float maxDistance = -1, bool findEnemies = true) where T : SidedEntity {
        SidedEntity obj = null;
        float f = float.PositiveInfinity;
        Predicate<MapObject> predicate = (findEnemies ? unit.getTeam().predicateOtherTeam : unit.getTeam().predicateThisTeam);
        foreach(SidedEntity s in unit.map.findMapObjects(predicate)) {
            if(s is T && !s.isDead()) {
                float dis = Vector3.Distance(point, s.transform.position);
                if((dis < f) && (maxDistance == -1 || dis < maxDistance)) {
                    obj = s;
                    f = dis;
                }
            }
        }
        return (T)obj;
    }

    /// <summary>
    /// Checks if the passed MapObject is within the passed units to this task's unit.
    /// </summary>
    protected bool inRange(MapObject target, float maxDistance) {
        return getDistance(target) <= maxDistance;
    }

    /// <summary>
    /// Returns the distance between this unit and the passed MapObject.
    /// </summary>
    protected float getDistance(MapObject other) {
        return Vector3.Distance(unit.getFootPos(), other.getPos());
    }

    protected float getDistance(Vector3 point) {
        return Vector3.Distance(unit.getFootPos(), point);
    }

    ///// <summary>
    ///// Checks if the unit is next to the passed building.
    ///// </summary>
    //protected bool nextToBuilding(BuildingBase building) {
    //    Vector2 v = building.getFootprintSize();
    //    Bounds b = new Bounds(building.getPos(), new Vector3(v.x + 1f, 4, v.y + 1f));
    //    return b.Intersects(unit.GetComponent<Collider>().bounds);
    //}

    public abstract bool preform();

    public void onFinish() {
        // There is no implementation, so there is no need to call super from implementations.
    }

    /// <summary>
    /// Called when the unit is damaged.
    /// </summary>
    public virtual void onDamage(MapObject dealer) {
        // There is no implementation, so there is no need to call super from implementations.
    }

    public virtual void drawDebug() {
        // There is no implementation, so there is no need to call super from implementations.
    }

    public virtual bool cancelable() {
        return true;
    }
}
