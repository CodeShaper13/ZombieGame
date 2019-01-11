using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    private MapData mapData;
    private Map map;
    private AvailableTeams availibleTeams;

    private void Awake() {
        References.list.registerPrefabsToNetworkManager(this);
    }

    public override void OnStartServer() {
        base.OnStartServer();

        this.mapData = GameObject.FindObjectOfType<MapData>();
        this.map = this.mapData.map;
        this.availibleTeams = new AvailableTeams(this.mapData.getPlayerCount());
    }

    public override void OnStopServer() {
        base.OnStopServer();

        Logger.log("Shutting down server...");

        this.map.saveMap();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Team team = this.availibleTeams.getAvailableTeam();
        int teamId = team.getId();

        Vector3? spawnPos = this.mapData.getSpawnPosFromTeam(team.getEnum());
        GameObject playerGameObj = GameObject.Instantiate(this.playerPrefab, spawnPos == null ? Vector3.zero : (Vector3)spawnPos, Quaternion.identity);
        Player player = playerGameObj.GetComponent<Player>();
        NetworkServer.AddPlayerForConnection(conn, playerGameObj, playerControllerId);

        // Set the Players Team.
        player.team = team;
        player.RpcSetTeam(teamId);

        ConnectedPlayer cp = new ConnectedPlayer(conn, player);

        if(this.map.gameSaver.doesPlayerSaveExist(player)) {
            this.map.gameSaver.readPlayerFromFile(player);
        } else {
            // Setup a first time player
            player.currentTeamResources = Constants.STARTING_RESOURCES;

            this.map.setupBase(this.mapData, team);
        }

        cp.sendMessage(new MessageChangeGameState(this.map.gameState));

        this.map.allPlayers.Add(cp);
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);

        //TODO use the method in Map to get the connected player.
        for(int i = this.map.allPlayers.Count - 1; i >= 0; i--) {
            ConnectedPlayer connectedPlayer = this.map.allPlayers[i];
            if(connectedPlayer.getConnection().connectionId == conn.connectionId) {
                this.map.allPlayers.RemoveAt(i);
                this.map.savePlayer(connectedPlayer);
                this.availibleTeams.freeTeam(connectedPlayer.getTeam());
                return;
            }
        }
    }
}
