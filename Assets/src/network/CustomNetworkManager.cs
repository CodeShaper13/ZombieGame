using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public bool isSinglePlayer;

    // Set from the title screen.  Used in randomly generated worlds
    public MapData mapData;
    public CampaignLevelData campaignData;
    
    private MapBase map;

    [SerializeField]
    private NetworkDiscovery discovery;

    private void Start() {
        //print("CNM.Start()");
        References.list.registerPrefabsToNetworkManager(this);

        //this.connections = new List<NetworkConnection>();
    }

    public override void OnServerSceneChanged(string sceneName) {
        base.OnServerSceneChanged(sceneName);

        //print("CNM.OnServerSceneChange()");

        // A scene has been loaded containing the level.  Setup everything.
        this.map = GameObject.FindObjectOfType<MapBase>();

        if(this.isSinglePlayer) {
            ((MapSP)this.map).setCampaignData(this.campaignData);
        }

        this.map.initialize(this.mapData);
    }

    public override void OnStopServer() {
        base.OnStopServer();

        Logger.log("Shutting down server...");

        if(this.discovery.running) {
            this.discovery.StopBroadcast();
        }

        if(!this.isSinglePlayer) {
            ((MapMP)this.map).saveMap();
        }
    }

    //public List<NetworkConnection> connections;

    public override void OnClientConnect(NetworkConnection conn) {
        //print("CM.OnClientConnect()");

        /*
        if(NetworkServer.active) {
            connections.Add(conn);
        } else {
            //ClientScene.AddPlayer(conn, 0);
        }
        */
    }

    public override void OnClientSceneChanged(NetworkConnection conn) {
        base.OnClientSceneChanged(conn);

        //print("CM.OnClientSceneChange()");

        //ClientScene.Ready(conn);

        this.map = GameObject.FindObjectOfType<MapBase>();

        //by overriding this function and commenting the base we are removing the functionality of this function 
        //so we dont have conflict with OnClientConnect
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        //print("CM.OnServerAddPlayer()");
        if(this.isSinglePlayer) {
            GameObject playerGameObj = GameObject.Instantiate(this.playerPrefab, Vector3.zero, Quaternion.identity);
            Player player = playerGameObj.GetComponent<Player>();
            NetworkServer.AddPlayerForConnection(conn, playerGameObj, playerControllerId);

            // Set the Players Team.
            player.team = Team.SURVIVORS_1;
            player.RpcSetTeam(Team.SURVIVORS_1.getId());

            this.map.allPlayers.Add(new ConnectedPlayer(conn, player));

            // Hacky way to update resources for the player/client
            this.map.setResources(Team.SURVIVORS_1, this.map.getResources(Team.SURVIVORS_1));
        }
        else {
            MapMP mpMap = (MapMP)this.map;

            Team team = mpMap.availibleTeams.getAvailableTeam();
            int teamId = team.getId();

            GameObject playerGameObj = GameObject.Instantiate(this.playerPrefab, Vector3.zero, Quaternion.identity);
            Player player = playerGameObj.GetComponent<Player>();
            NetworkServer.AddPlayerForConnection(conn, playerGameObj, playerControllerId);

            // Set the Players Team.
            player.team = team;
            player.RpcSetTeam(teamId);

            ConnectedPlayer cp = new ConnectedPlayer(conn, player);

            if(mpMap.gameSaver.doesPlayerSaveExist(player)) {
                mpMap.gameSaver.readPlayerFromFile(player);
            }
            else {
                // Setup a first time player
                mpMap.setResources(team, Constants.STARTING_RESOURCES);

                // TODO spawn starting units?
                //mpMap.mapGenerator.setupPlayerBase(team);
            }
            this.map.allPlayers.Add(cp);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);

        if(!this.isSinglePlayer) {
            MapMP mpMap = (MapMP)this.map;

            //TODO use the method in Map to get the connected player.
            for(int i = this.map.allPlayers.Count - 1; i >= 0; i--) {
                ConnectedPlayer connectedPlayer = this.map.allPlayers[i];
                if(connectedPlayer.getConnection().connectionId == conn.connectionId) {
                    mpMap.allPlayers.RemoveAt(i);
                    mpMap.savePlayer(connectedPlayer);
                    mpMap.availibleTeams.freeTeam(connectedPlayer.getTeam());
                    return;
                }
            }
        }
    }
}
