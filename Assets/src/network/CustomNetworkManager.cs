using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    private MapData mapData;

    public AvailableTeams availibleTeams;

    public override void OnStartServer() {
        base.OnStartServer();

        this.mapData = GameObject.FindObjectOfType<MapData>();

        this.availibleTeams = new AvailableTeams(this.mapData.teamBaseData.Length);
        foreach(TeamBaseData data in this.mapData.teamBaseData) {
            Team.getTeamFromEnum(data.team).setOrgin(data.orginPoint);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Team team = this.availibleTeams.getAvailableTeam();
        int teamId = team.getTeamId();
        
        GameObject playerObject = GameObject.Instantiate(this.playerPrefab, team.getOrginPos(), Quaternion.identity);
        Player player = playerObject.GetComponent<Player>();
        NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);

        player.team = team;
        player.RpcSetTeam(teamId);

        MessageChangeGameState msg = new MessageChangeGameState(Map.instance.gameState);
        conn.Send(msg.getID(), msg);

        Map.instance.allPlayers.Add(player);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
        base.OnServerRemovePlayer(conn, playerController);

        Player player = playerController.gameObject.GetComponent<Player>();
        this.availibleTeams.freeTeam(player.team);

        Map.instance.allPlayers.Remove(player);
    }
}
