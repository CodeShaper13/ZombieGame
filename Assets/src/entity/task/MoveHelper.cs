using System;
using UnityEngine;

public class MoveHelper {

    private const float TURN_SPEED = 500f;

    private UnitBase unit;
    private NavAgent2D agent;

    private Vector3 lastSetCall;

    public MoveHelper(UnitBase unit) {
        this.unit = unit;
        this.agent = this.unit.GetComponent<NavAgent2D>();
    }

    /// <summary>
    /// Rotates the Unit's body and head to look at the passed position.
    /// </summary>
    public void lookAt(Vector3 pos) {
        throw new NotImplementedException();
    }

    public void setDestination(LivingObject entity) {
        this.setDestination(entity.getPos(), entity.getSizeRadius() - this.unit.getSizeRadius());
    }

    public void setDestination(Vector3 pos, float stopingDistance = -1) {
        if(pos != lastSetCall) {
            this.agent.setDestination(pos);

            if(stopingDistance != -1) {
                this.agent.stoppingDistance = stopingDistance;
            }
            else {
                this.agent.stoppingDistance = 0.25f; //TODO should this be a setting?
            }
        }

        lastSetCall = pos;
    }

    /// <summary>
    /// Stops the unit where they are.
    /// </summary>
    public void stop() {
        if(this.agent.hasPath()) {
            this.agent.stop();
        }
    }

    /*
    public void drawDebug() {
        GLDebug.DrawLine(agent.transform.position, agent.destination.setY(0.5f), Colors.magenta);

        NavMeshPath path = this.agent.path;
        for(int i = 0; i < path.corners.Length - 1; i++) {
            GLDebug.DrawLine(path.corners[i], path.corners[i + 1], Colors.pink);
        }
    }
    */
}
