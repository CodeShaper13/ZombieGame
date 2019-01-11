using fNbt;
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

    [ServerSideOnly]
    private NetHandlerServer handler;    
    [ServerSideOnly]
    public GameSaver gameSaver;
    /// <summary> A List of all the connected players. </summary>
    [ServerSideOnly]
    public List<ConnectedPlayer> allPlayers;

    private Transform holderHarvestable;
    private Transform holderBuilding;
    private Transform holderUnit;
    private Transform holderProjectile;

    private void Awake() {
        Map.instance = this;

        this.holderHarvestable = this.createHolderObject("HARVESTABLE");
        this.holderBuilding = this.createHolderObject("BUILDING");
        this.holderUnit = this.createHolderObject("UNIT");
        this.holderProjectile = this.createHolderObject("PROJECTILE");
    }

    public override void OnStartServer() {
        base.OnStartServer();

        this.initialize();

        // Spawn all of the objects that were created durring map generation/save loading.
        foreach(MapObject obj in this.mapObjects) {
            NetworkServer.Spawn(obj.gameObject);
        }
    }

    public override void OnStartClient() {
        if(!this.isServer) {
            this.mapObjects = new List<MapObject>();
        }
    }

    /// <summary>
    /// Initializes the Map, getting it ready to use and either loading it from file or creating a new one.
    /// </summary>
    [ServerSideOnly]
    public void initialize() {
        this.mapObjects = new List<MapObject>();
        this.allPlayers = new List<ConnectedPlayer>();

        this.handler = new NetHandlerServer(this);

        // Setup state of game.
        this.timer = 60f;
        this.gameState = EnumGameState.PLAY;

        this.gameSaver = new GameSaver("game1");

        // If a save game exits, read it form file.
        if(this.gameSaver.doesSaveExists()) {
            this.loadMap();
        }
        else { // Otherwise create a new game.
            Logger.log("No save found, creating a new Map...");
            this.newMap(GameObject.FindObjectOfType<MapData>());
        }
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
    public SpawnInstructions<T> spawnEntity<T>(RegisteredObject registeredObj, Vector3 position, Quaternion? rotation = null, Vector3? scale = null) where T : MapObject {
        if(rotation == null) {
            rotation = Quaternion.identity;
        }
        GameObject gameObj = GameObject.Instantiate(registeredObj.getPrefab(), position, (Quaternion)rotation);
        if(scale != null) {
            gameObj.transform.localScale = (Vector3)scale;
        }
        T newObject = func<T>(gameObj);

        return new SpawnInstructions<T>(newObject);
    }

    [ServerSideOnly]
    public SpawnInstructions<T> spawnEntity<T>(RegisteredObject registeredObj, NbtCompound tag) where T : MapObject {
        GameObject gameObj = GameObject.Instantiate(registeredObj.getPrefab());
        T mapObject = this.func<T>(gameObj);

        mapObject.guid = tag.getGuid("guid");

        return new SpawnInstructions<T>(mapObject);
    }

    private T func<T>(GameObject gameObj) where T : MapObject {
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

    private Player playerFromTeam(Team team) {
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

    #endregion

    /// <summary>
    /// Saves the Map and all connected Players to file by calling Map#savePlayer().
    /// </summary>
    [ServerSideOnly]
    public void saveMap() {
        Logger.log("Saving Map to disk...");
        this.gameSaver.saveMapToFile(this);
        foreach(ConnectedPlayer cPlayer in this.allPlayers) {
            this.savePlayer(cPlayer);
        }
    }

    /// <summary>
    /// Saves the passed Player to file.
    /// </summary>
    [ServerSideOnly]
    public void savePlayer(ConnectedPlayer cPlayer) {
        Logger.log("Saving Player for team " + cPlayer.getTeam().getTeamName() + " to disk...");
        this.gameSaver.savePlayerToFile(cPlayer.getPlayer());
    }

    [ServerSideOnly]
    public void loadMap() {
        this.gameSaver.readMapFromFile(this);
    }

    [ServerSideOnly]
    public void newMap(MapData mapData) {
        MapGenerator generator = MapGenerator.getGeneratorFromEnum(mapData.mapGenerator, this, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        if(generator != null) {
            generator.generateMap(mapData);
        }
    }

    /// <summary>
    /// Places objects to set up a base for new Players. (The builder)
    /// </summary>
    [ServerSideOnly]
    public void setupBase(MapData mapData, Team team) {
        float angle = UnityEngine.Random.Range(0, 359);
        Vector3 position = (Vector3)mapData.getSpawnPosFromTeam(team.getEnum()) + (Quaternion.Euler(0, angle, 0) * new Vector3(2, 0, 0));
        SpawnInstructions<SidedEntity> instructions = this.spawnEntity<SidedEntity>(Registry.unitBuilder, position);
        instructions.getObj().setTeam(team);
        instructions.spawn();
    }

    /// <summary>
    /// Writes the map to the passed NbtCompound.
    /// </summary>
    [ServerSideOnly]
    public void writeToNbt(NbtCompound tag) {
        tag.setTag("timer", this.timer);
        tag.setTag("gameState", (int)this.gameState);

        // Write the map objects to NBT
        NbtList nbtMapObjList = new NbtList("mapObjects", NbtTagType.Compound);
        foreach(MapObject mapObject in this.mapObjects) {
            NbtCompound tagCompound = new NbtCompound();
            mapObject.writeToNbt(tagCompound);
            nbtMapObjList.Add(tagCompound);
        }
        tag.Add(nbtMapObjList);
    }

    [ServerSideOnly]
    public void readFromNbt(NbtCompound tag) {
        this.timer = tag.getFloat("timer");
        this.gameState = (EnumGameState)tag.getInt("gameState");

        foreach(NbtCompound compound in tag.getList("mapObjects")) {
            int id = compound.getInt("id");
            RegisteredObject registeredObject = Registry.getObjectFromRegistry(id);
            if(registeredObject != null) {
                SpawnInstructions<MapObject> instructions = this.spawnEntity<MapObject>(registeredObject, compound);
                instructions.getObj().map = this;
                instructions.getObj().nbtTag = compound;
            } else {
                Logger.logError("MapObject with an unknown ID of " + id + " was found!  Ignoring!");
            }
        }

        // Now that every MapObject is loaded and their Guid is set, preform the normal reading from nbt.
        foreach(MapObject obj in this.mapObjects) {
            obj.readFromNbt(obj.nbtTag);
        }
    }

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
}
