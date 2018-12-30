using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Map : NetworkBehaviour {

    public static Map instance;

    [SyncVar]
    [HideInInspector]
    public float timer;
    public EnumGameState gameState = EnumGameState.PREPARE;

    public List<MapObject> mapObjects;
    public NetHandlerServer handler;
    
    public MapData mapData;

    /// <summary>
    /// A List of all the connected players.
    /// </summary>
    [ServerSideOnly]
    public List<ConnectedPlayer> allPlayers;

    private void Awake() {
        Map.instance = this;

        this.mapData = GameObject.FindObjectOfType<MapData>();
    }

    public override void OnStartClient() {
        if(!this.isServer) {
            this.mapObjects = new List<MapObject>();
        }
    }

    public override void OnStartServer() {
        this.mapObjects = new List<MapObject>();
        this.allPlayers = new List<ConnectedPlayer>();

        this.handler = new NetHandlerServer(this);

        // Setup state of game.
        this.timer = this.mapData.setupTime;
        this.gameState = EnumGameState.PLAY;
    }

    private void Update() {
        if(this.isServer) {
            // Debug.
            if(Input.GetKeyDown(KeyCode.F2)) {
                this.startBattle();
            }

            if(this.gameState == EnumGameState.PREPARE) {
                this.timer -= Time.deltaTime;
                if(this.timer <= 0f) {
                    this.startBattle();
                }
            } else if(this.gameState == EnumGameState.PLAY) {
                for(int i = this.mapObjects.Count - 1; i >= 0; i--) {
                    this.mapObjects[i].onUpdate(Time.deltaTime);
                }

                //Check if someone has won.
            } else if(this.gameState == EnumGameState.END) {

            }
        }
    }

    private void LateUpdate() {
        if(this.isClient && Main.DEBUG) {
            for(int i = this.mapObjects.Count - 1; i >= 0; i--) {
                this.mapObjects[i].drawDebug();
            }
        }
    }

    private void startBattle() {
        this.gameState = EnumGameState.PLAY;
        this.timer = -1f;
        this.sendMessageToAll(new MessageChangeGameState(EnumGameState.PLAY));
        this.sendMessageToAll(new MessageShowAnnouncement("Let the Battle Begin!", 2f));
    }

    /// <summary>
    /// Fires a projectile.  This should only be called on the server!
    /// </summary>
    [ServerSideOnly]
    public Projectile fireProjectile(Vector3 position, SidedEntity shooter, int damage, LivingObject target) {
        GameObject obj = GameObject.Instantiate(
            Registry.projectileArrow.getPrefab(),
            position,
            Quaternion.identity);
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.setProjectileInfo(shooter, damage, target);

        NetworkServer.Spawn(obj);

        return projectile;
    }

    /// <summary>
    /// Spawns a new MapObject and returns it wrpaped in a SpawnInstructions instance.
    /// Be sure to call the spawn method AFTER modifying the newly spawned MapObject.
    /// </summary>
    [ServerSideOnly]
    public SpawnInstructions<T> spawnEntity<T>(RegisteredObject registerObj, Vector3 position, Quaternion? rotation = null, Vector3? scale = null) where T : MapObject {
        if(rotation == null) {
            rotation = Quaternion.identity;
        }
        GameObject entity = GameObject.Instantiate(registerObj.getPrefab(), position, (Quaternion)rotation);
        if(scale != null) {
            entity.transform.localScale = (Vector3)scale;
        }

        T newObject = entity.GetComponent<T>();
        this.mapObjects.Add(newObject);

        return new SpawnInstructions<T>(newObject);
    }

    /// <summary>
    /// Returns a list of MapObjects on the Map that match the passed predicate.
    /// Pass null to get all MapObjects.  Note, a copy of the list is NOT made.
    /// </summary>
    public List<MapObject> findMapObjects(Predicate<MapObject> predicate) {
        if(predicate == null) {
            return this.mapObjects; //TODO should a copy be returned?
        } else {
            return this.mapObjects.FindAll(predicate);
        }
    }

    public MapObject findMapObjectFromGuid(Guid guid) {
        foreach(MapObject obj in this.mapObjects) {
            if(obj.getGuid() == guid) {
                return obj;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the closest MapObject to the passed point.
    /// </summary>
    public MapObject findClosestObject(Vector3 point, Predicate<MapObject> validPredicate) {
        return Util.closestToPoint(point, this.mapObjects, validPredicate);
    }

    [ServerSideOnly]
    public void removeEntity(MapObject mapObj, bool haveDeathEffect = true) {
        this.mapObjects.Remove(mapObj);

        if (haveDeathEffect) {
            if(mapObj is LivingObject) {
                ((LivingObject)mapObj).onDeath();
            }
        }

        GameObject.Destroy(mapObj.gameObject);
        //NetworkServer.Destroy(mapObj.gameObject);
    }

    /// <summary>
    /// Sends a message to all of the connected players.
    /// </summary>
    [ServerSideOnly]
    public void sendMessageToAll(AbstractMessage<NetHandlerClient> message) {
        foreach(ConnectedPlayer cp in this.allPlayers) {
            cp.sendMessage(message);
        }
    }

    private Player playerFromTeam(Team team) {
        foreach(ConnectedPlayer cp in this.allPlayers) {
            if(cp.getTeam() == team) {
                return cp.getPlayer();
            }
        }
        return null;
    }

    [ServerSideOnly]
    public void reduceResources(Team team, int amount) {
        this.playerFromTeam(team).currentTeamResources -= amount;
    }

    [ServerSideOnly]
    public void increaceResources(Team team, int amount) {
        this.playerFromTeam(team).currentTeamResources += amount;
    }

    [ServerSideOnly]
    public int getResources(Team team) {
        return this.playerFromTeam(team).currentTeamResources;
    }
}
