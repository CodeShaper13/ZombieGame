using UnityEngine;

public class TeamGround : MonoBehaviour {

    [SerializeField]
    private EnumTeam team;

    /// <summary>
    /// Returns true if the passed Player can preform actions on this ground.
    /// </summary>
    public bool canInteractWith(Player player) {
        return this.team == EnumTeam.NONE || this.team == player.getTeam().getEnum();
    }
}
