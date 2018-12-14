using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public Transform[] teamSpawnPoints;

    private AvailableTeams availibleTeams;

    public override void OnStartServer() {
        base.OnStartServer();

        this.availibleTeams = new AvailableTeams(2);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Team team = this.availibleTeams.getAvailableTeam();
        int teamId = team.getTeamId();

        Vector3 startPos;
        if (teamId > this.teamSpawnPoints.Length) {
            startPos = Vector3.zero;
        } else {
            startPos = this.teamSpawnPoints[teamId - 1].position;
        }
        GameObject playerObject = GameObject.Instantiate(this.playerPrefab, startPos, Quaternion.identity);
        Player player = playerObject.GetComponent<Player>();
        NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);

        player.team = team;
        player.RpcSetTeam(teamId);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player) {
        base.OnServerRemovePlayer(conn, player);

        this.availibleTeams.freeTeam(player.gameObject.GetComponent<Player>().team);
    }
}
