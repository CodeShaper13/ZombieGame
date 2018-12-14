using UnityEngine;

public class TeamGround : MonoBehaviour {

    [SerializeField]
    private EnumTeam team;
    private Team teamObj;

    private void Awake() {
        this.teamObj = Team.getTeamFromEnum(this.team);
    }

    public Team getTeam() {
        return this.teamObj;
    }
}
