using UnityEngine;
using UnityEngine.Networking;

public abstract class SidedEntity : MapObject {

    private Team team;

    public Team getTeam() {
        return this.team;
    }

    public void setTeam(Team team) {
        this.team = team;
    }
    [ClientRpc]
    public void RpcSetTeam(int newTeamId) {
        this.team = Team.getTeamFromId(newTeamId);
        this.colorObject();
    }

    /// <summary>
    /// Colors this object based on it's team.
    /// </summary>
    public virtual void colorObject() {
        Color color = this.team.getTeamColor();
        this.GetComponent<MeshRenderer>().material.color = color;
    }

    public SidedEntity getClosestEnemyObject() {
        SidedEntity closest = null;
        float d = float.PositiveInfinity;
        foreach (SidedEntity s in this.map.mapObjects) {
            if(s.team != this.team) {
                float f = Vector3.Distance(this.transform.position, s.transform.position);
                if (f < d) {
                    d = f;
                    closest = s;
                }
            }
        }
        return closest;
    }
}
