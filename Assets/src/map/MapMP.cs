using UnityEngine;
using System.Collections.Generic;
using System;
using fNbt;
using UnityEngine.Networking;

public class MapMP : MapBase {

    [ServerSideOnly]
    public AvailableTeams availibleTeams;
    [ServerSideOnly]
    public GameSaver gameSaver;

    public override void initialize(MapData mapData) {
        base.initialize(mapData);

        this.gameSaver = new GameSaver(this.mapData.saveName);

        this.availibleTeams = new AvailableTeams(this.getPlayerCount());

        if(this.gameSaver.doesSaveExists()) {
            // Load game.
            Logger.log("Loading Save Game...");
            this.gameSaver.readMapFromFile(this);
        } else {
            // New game.
            Logger.log("No Save found, creating a new Map...");

            // TODO new map?

            // Spawn starting units:
            SpawnInstructions<UnitBase> unit;
            unit = this.spawnEntity<UnitBase>(Registry.unitGunner, new Vector3(0, 0));
            unit.getObj().setTeam(Team.SURVIVORS_1);
            unit = this.spawnEntity<UnitBase>(Registry.unitSoldier, new Vector3(0, 1));
            unit.getObj().setTeam(Team.SURVIVORS_1);
            unit = this.spawnEntity<UnitBase>(Registry.unitBuilder, new Vector3(2, -1));
            unit.getObj().setTeam(Team.SURVIVORS_1);

            // Spawn zombies:
            Vector2Int origin = TileMaps.singleton.getCombinedOrigin();
            Vector2Int size = TileMaps.singleton.getCombinedSize();
            for(int i = 0; i < 25; i++) {
                int x = UnityEngine.Random.Range(origin.x + 2, size.x - 1);
                int y = UnityEngine.Random.Range(origin.y + 2, size.y - 1);
                unit = this.spawnEntity<UnitBase>(Registry.zombie, new Vector3(x, y));
                unit.getObj().setTeam(Team.ZOMBIES);
            }
        }

        // Spawn all of the objects that were created durring map generation/save loading.
        foreach(MapObject obj in this.mapObjects) {
            NetworkServer.Spawn(obj.gameObject);
        }
    }

    /// <summary>
    /// Saves the Map and all connected Players to file by calling Map#savePlayer().
    /// </summary>
    [ServerSideOnly]
    public void saveMap() {
        Logger.log("Saving Map to disk...");

        this.gameSaver.saveMapDataToFile(this.mapData);
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

    /// <summary>
    /// Writes the map to the passed NbtCompound.
    /// </summary>
    [ServerSideOnly]
    public void writeToNbt(NbtCompound tag) {
        tag.setTag("resources", this.resources);        

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
        this.resources = tag.getIntArray("resources");        

        foreach(NbtCompound compound in tag.getList("mapObjects")) {
            int id = compound.getInt("id");
            RegisteredObject registeredObject = Registry.getObjectFromRegistry(id);
            if(registeredObject != null) {
                SpawnInstructions<MapObject> instructions = this.spawnEntity<MapObject>(registeredObject, compound);
                instructions.getObj().map = this;
                instructions.getObj().nbtTag = compound;
            }
            else {
                Logger.logError("MapObject with an unknown ID of " + id + " was found!  Ignoring!");
            }
        }

        // Now that every MapObject is loaded and their Guid is set, preform the normal reading from nbt.
        foreach(MapObject obj in this.mapObjects) {
            obj.readFromNbt(obj.nbtTag);
        }
    }

    public override int getPlayerCount() {
        return 4;
    }
}
