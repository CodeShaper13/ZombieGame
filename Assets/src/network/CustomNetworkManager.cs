using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    private MapData mapData;

    public AvailableTeams availibleTeams;
    private Map map;

    private void Awake() {
        References.list.func(this);

        this.map = GameObject.Instantiate(References.list.mapPrefab).GetComponent<Map>();
    }

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
        int teamId = team.getId();
        
        GameObject playerGameObj = GameObject.Instantiate(this.playerPrefab, team.getOrginPos(), Quaternion.identity);
        Player player = playerGameObj.GetComponent<Player>();
        NetworkServer.AddPlayerForConnection(conn, playerGameObj, playerControllerId);

        player.team = team;
        player.RpcSetTeam(teamId);

        ConnectedPlayer cp = new ConnectedPlayer(conn, player);

        // Setup a first time player
        player.currentTeamResources = Constants.STARTING_RESOURCES;

        cp.sendMessage(new MessageChangeGameState(this.map.gameState));

        this.map.allPlayers.Add(cp);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
        base.OnServerRemovePlayer(conn, playerController);

        Player player = playerController.gameObject.GetComponent<Player>();
        this.availibleTeams.freeTeam(player.team);

        for(int i = this.map.allPlayers.Count - 1; i >= 0; i--) {
            // Do processing here, then...
            ConnectedPlayer cp = this.map.allPlayers[i];
            if(cp.getConnection().connectionId == conn.connectionId) {
                this.map.allPlayers.RemoveAt(i);
                return;
            }
        }

        Debug.Log("PROBLEM!");
    }
}
