public class AvailableTeams {

    private bool[] availibleTeams;
    private int totalTeams;

    public AvailableTeams(int totalTeams) {
        this.totalTeams = totalTeams;
        this.availibleTeams = new bool[this.totalTeams];
        for(int i = 0; i < this.totalTeams; i++) {
            this.availibleTeams[i] = true;
        }
    }

    /// <summary>
    /// Team.NONE is returned if there is no free team.
    /// </summary>
    public Team getAvailableTeam() {
        for(int i = 0; i < this.totalTeams; i++) {
            if(this.availibleTeams[i] == false) {
                continue;
            } else {
                this.availibleTeams[i] = false;
                return Team.getTeamFromId(i + 1); // Team.NONE is 0, the first team is ID 1.
            }
        }
        return Team.NONE;
    }

    /// <summary>
    /// Call to free the passed Team up from the list of Teams.
    /// </summary>
    public void freeTeam(Team team) {
        int i = team.getId();
        this.availibleTeams[i - 1] = true;
    }
}
