using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveHelper : IDrawDebug {

    private const float TURN_SPEED = 500f;

    private UnitBase unit;
    private NavMeshAgent agent;

    private Vector3 lastSetCall;

    public MoveHelper(UnitBase unit) {
        this.unit = unit;
        agent = this.unit.GetComponent<NavMeshAgent>();
        agent.speed *= this.unit.getData().baseSpeedMultiplyer;
        agent.angularSpeed = TURN_SPEED;
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
            agent.isStopped = false;
            agent.SetDestination(pos);

            if(stopingDistance != -1) {
                agent.stoppingDistance = stopingDistance;
            }
            else {
                agent.stoppingDistance = 0.25f; //TODO should this be a setting?
            }
        }

        lastSetCall = pos;
    }

    /// <summary>
    /// Stops the unit where they are.  If stopImmediately is true, the agent's velocity is set to 0, freezing it instantly.
    /// </summary>
    public void stop(bool stopImmediately = false) {
        if(!agent.isStopped) {
            agent.isStopped = true;
            if(stopImmediately) {
                agent.velocity = Vector3.zero;
            }
        }
    }

    public void drawDebug() {
        GLDebug.DrawLine(agent.transform.position.setY(0.5f), agent.destination.setY(0.5f), Colors.magenta);
    }
}
