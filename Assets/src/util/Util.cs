using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

    /// <summary>
    /// Returns true if the passed LivingObject is both not null and alive.
    /// </summary>
    public static bool isAlive(LivingObject obj) {
        return obj != null && !obj.isDead();
    }

    /// <summary>
    /// Returns the closest map object to the passed point.
    /// </summary>
    public static T closestToPoint<T>(Vector3 point, IEnumerable<T> list) where T : MapObject {
        return Util.closestToPoint<T>(point, list, null);
    }

    /// <summary>
    /// Returns the closest MapObject in the list.  this may return null if validOptionFunc
    /// returns false for every member of the list, or if the list is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="point">The point to compare the entity's point to.</param>
    /// <param name="list"></param>
    /// <param name="validPredicate">If not null, this function is called on every entity
    /// in the list.  If this function returns false the entity is not considered to be in
    /// the running for the closest.</param>
    /// <returns></returns>
    public static T closestToPoint<T>(Vector3 point, IEnumerable<T> list, Predicate<T> validPredicate) where T : MapObject {
        T bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        float dis;
        foreach(T obj in list) {
            dis = Vector3.Distance(point, obj.getPos());
            if(dis < closestDistanceSqr) {
                if(validPredicate != null && !validPredicate(obj)) {
                    continue;
                }
                bestTarget = obj;
                closestDistanceSqr = dis;
            }
        }
        return bestTarget;
    }
}
