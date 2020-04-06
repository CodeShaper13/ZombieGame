using fNbt;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class MapBase : NetworkBehaviour, IDrawDebug {

    public static MapBase instance;

    /// <summary> A list of all of the map objects on the mpa.  This exists on both the client and server side. </summary>
    public List<MapObject> mapObjects;

    /// <summary> A List of all the connected players. </summary>
    [ServerSideOnly]
    public List<ConnectedPlayer> allPlayers;

    protected int[] resources;

    [ServerSideOnly]
    private NetHandlerServer handler;

    public MapData mapData;

    // These are avalible on both the client and server side.
    private Transform holderHarvestable;
    private Transform holderBuilding;
    private Transform holderUnit;
    private Transform holderProjectile;

    private void Awake() {
        MapBase.instance = this;

        this.mapObjects = new List<MapObject>();
        this.allPlayers = new List<ConnectedPlayer>();

        // Create the MapObject organization objects.
        this.holderHarvestable = this.createHolderObject("HARVESTABLE");
        this.holderBuilding = this.createHolderObject("BUILDING");
        this.holderUnit = this.createHolderObject("UNIT");
        this.holderProjectile = this.createHolderObject("PROJECTILE");

        // Setup and sort all of the map objects.
        foreach(MapObject mapObj in GameObject.FindObjectsOfType<MapObject>()) {
            this.setupMapObj<MapObject>(mapObj.gameObject);
        }

        this.onAwake();
    }

    protected virtual void onAwake() { }

    public override void OnStartClient() {
        if(!this.isServer) {
            this.mapObjects = new List<MapObject>();
        }
    }

    /// <summary>
    /// Initializes the Map, getting it ready to use and either loading it from file or creating a new one.
    /// </summary>
    [ServerSideOnly]
    public virtual void initialize(MapData mapData) {
        this.mapData = mapData;

        this.handler = new NetHandlerServer(this);
        this.resources = new int[Team.ALL_TEAMS.Length];
    }

    private void Update() {
        if(this.isServer) {
            for(int i = this.mapObjects.Count - 1; i >= 0; i--) {
                this.mapObjects[i].onUpdate(Time.deltaTime);
            }
        }

        this.updateMap();
    }

    protected virtual void updateMap() { }

    public abstract int getPlayerCount();

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
    public SpawnInstructions<T> spawnEntity<T>(RegisteredObject registeredObj, Vector3 position, Quaternion? rotation = null, Vector3? scale = null) where T : MapObject {
        if(rotation == null) {
            rotation = Quaternion.identity;
        }
        GameObject gameObj = GameObject.Instantiate(registeredObj.getPrefab(), position, (Quaternion)rotation);
        if(scale != null) {
            gameObj.transform.localScale = (Vector3)scale;
        }
        T newObject = setupMapObj<T>(gameObj);

        return new SpawnInstructions<T>(newObject);
    }

    [ServerSideOnly]
    public SpawnInstructions<T> spawnEntity<T>(RegisteredObject registeredObj, NbtCompound tag) where T : MapObject {
        GameObject gameObj = GameObject.Instantiate(registeredObj.getPrefab());
        T mapObject = this.setupMapObj<T>(gameObj);

        mapObject.guid = tag.getGuid("guid");

        return new SpawnInstructions<T>(mapObject);
    }

    [ServerSideOnly]
    protected T setupMapObj<T>(GameObject gameObj) where T : MapObject {
        T newObject = gameObj.GetComponent<T>();
        newObject.onServerAwake();
        this.addMapObject(newObject);

        return newObject;
    }

    #region MapObject finders

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

    /// <summary>
    /// Finds a MapObject by it's GUID.  If no MapObject is found, null is returned.
    /// </summary>
    public T findMapObjectFromGuid<T>(Guid guid) where T : MapObject {
        if(guid == Guid.Empty) {
            return null; // When reading from NBT these are commonly the default.
        }

        foreach(MapObject obj in this.mapObjects) {
            if(obj.getGuid() == guid) {
                return (T)obj;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the closest MapObject to the passed point.  Null is returned if there are no MapObjects on the map, of if the predicate fails for all.
    /// </summary>
    public MapObject findClosestObject(Vector3 point, Predicate<MapObject> validPredicate) {
        return Util.closestToPoint(point, this.mapObjects, validPredicate);
    }

    #endregion

    [ServerSideOnly]
    public void removeMapObject(MapObject mapObj, bool haveDeathEffect = true) {
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
    public void sendMessageToAll(AbstractMessageClient message) {
        foreach(ConnectedPlayer cp in this.allPlayers) {
            cp.sendMessage(message);
        }
    }

    /// <summary>
    /// Returns the player controlling the passed team.  Null is returned if no player controls that team.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    private Player getPlayerFromTeam(Team team) {
        foreach(ConnectedPlayer cp in this.allPlayers) {
            if(cp.getTeam() == team) {
                return cp.getPlayer();
            }
        }
        return null;
    }

    public ConnectedPlayer connectedPlayerFromNetworkConnection(NetworkConnection conn) {
        foreach(ConnectedPlayer connectedPlayer in this.allPlayers) {
            if(connectedPlayer.getConnection().connectionId == conn.connectionId) {
                return connectedPlayer;
            }
        }
        return null;
    }

    #region Resources Getters and Setters

    [ServerSideOnly]
    public void reduceResources(Team team, int amount) {
        //this.playerFromTeam(team).currentTeamResources
        this.setResources(team, this.getResources(team) - amount);
    }

    /// <summary>
    /// Transfers resources, returning any leftovers that don't fit.
    /// </summary>
    [ServerSideOnly]
    public int transferResources(Team team, int amount) {
        Player p = this.getPlayerFromTeam(team);

        //int maxTransferAmount = team.getMaxResourceCount(this) - p.currentTeamResources;
        int maxTransferAmount = team.getMaxResourceCount(this) - this.getResources(team);

        int amountToTransfer = Mathf.Min(amount, maxTransferAmount);
        //p.currentTeamResources += amountToTransfer;
        this.setResources(team, this.getResources(team) + amountToTransfer);
        amount -= amountToTransfer;

        return amount;
    }

    /// <summary>
    /// Sets a teams resources, clamping the value between 0 and the teams max.
    /// </summary>
    [ServerSideOnly]
    public void setResources(Team team, int amount) {
        //this.playerFromTeam(team).currentTeamResources
        int i = Mathf.Clamp(amount, 0, team.getMaxResourceCount(this));
        this.resources[team.getId()] = i;
        Player p = this.getPlayerFromTeam(team);
        if(p != null) {
            p.currentTeamResources = i;
        }
    }

    [ServerSideOnly]
    public int getResources(Team team) {
        //return this.playerFromTeam(team)
        return this.resources[team.getId()];
    }

    [ServerSideOnly]
    public bool canHoldMoreResources(Team team) {
        //return this.getResources(team)
        return this.getResources(team) < team.getMaxResourceCount(this);
    }

    #endregion

    /// <summary>
    /// Adds the passed MapObject to the list and places it in the right holder in the hierarchy.
    /// </summary>
    public void addMapObject(MapObject mapObj) {
        this.mapObjects.Add(mapObj);
        if(mapObj is HarvestableObject) {
            mapObj.transform.parent = this.holderHarvestable;
        } else if(mapObj is BuildingBase) {
            mapObj.transform.parent = this.holderBuilding;
        } else if(mapObj is UnitBase) {
            mapObj.transform.parent = this.holderUnit;
        } else if(mapObj is Projectile) {
            mapObj.transform.parent = this.holderProjectile;
        }
    }

    /// <summary>
    /// Creates or finds a Transfrom to hold GameObjects, used to keep the hierarchy organized.
    /// </summary>
    private Transform createHolderObject(string name) {
        string s = name + "_HOLDER";
        GameObject obj = GameObject.Find(s);
        if(obj == null) {
            obj = new GameObject(s);
        }
        return obj.transform;
    }

    public virtual void drawDebug() { }
}
