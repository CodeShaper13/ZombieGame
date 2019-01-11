using UnityEngine;

public class SpawnPosition : MonoBehaviour {

    [SerializeField]
    private EnumTeam team;

    private void OnDrawGizmos() {
        Gizmos.color = Team.getTeamFromEnum(this.team).getColor();
        Gizmos.DrawSphere(this.transform.position, 1f);
    }

    public EnumTeam getTeam() {
        return this.team;
    }
}
