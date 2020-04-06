using fNbt;
using System.IO;
using UnityEngine;

public class GameSaver {

    /// <summary> The extension of the save files. </summary>
    private const string EXTENSION = ".nbt";

    private bool dontWriteToDisk = false;

    private DirectoryInfo saveFolder;

    /// <summary>
    /// saveName should be just the name of the save, with no leading or trailing slashes.
    /// </summary>
    public GameSaver(string saveName) {
        this.saveFolder = new DirectoryInfo(Main.SAVE_DIR + "/" + saveName + "/");
        this.makeDirIfMissing(this.saveFolder);
    }

    /// <summary>
    /// Returns true if this save can be found on the disk.
    /// </summary>
    public bool doesSaveExists() {
        return File.Exists(this.getSaveFileName());
    }

    public MapData readMapDataFromFile() {
        string s = this.getMapDataFileName();
        if(File.Exists(s)) {
            NbtFile file = new NbtFile();
            file.LoadFromFile(s);

            NbtCompound rootTag = file.RootTag;
            return new MapData(rootTag);
        }

        return new MapData();
    }

    public void saveMapDataToFile(MapData mapData) {
        #if UNITY_EDITOR
            if(this.dontWriteToDisk) { return; }
        #endif

        NbtFile file = new NbtFile(mapData.writeToNbt());
        file.SaveToFile(this.getMapDataFileName(), NbtCompression.None);
    }

    /// <summary>
    /// Returns true if a save file exists for the passed Player.
    /// </summary>
    public bool doesPlayerSaveExist(Player player) {
        return File.Exists(this.getPlayerFileName(player));
    }

    public void readMapFromFile(MapMP map) {
        string s = this.getSaveFileName();
        if(File.Exists(s)) {
            NbtFile file = new NbtFile();
            file.LoadFromFile(s);

            NbtCompound rootTag = file.RootTag;
            map.readFromNbt(rootTag);
        }
    }

    public void saveMapToFile(MapMP map) {
        #if UNITY_EDITOR
            if(this.dontWriteToDisk) { return; }
        #endif

        NbtCompound rootTag = new NbtCompound("map");
        map.writeToNbt(rootTag);

        NbtFile file = new NbtFile(rootTag);
        file.SaveToFile(this.getSaveFileName(), NbtCompression.None);
    }

    public void readPlayerFromFile(Player player) {
        string s = this.getPlayerFileName(player);
        if(File.Exists(s)) {
            NbtFile file = new NbtFile();
            file.LoadFromFile(s);

            NbtCompound rootTag = file.RootTag;
            player.readFromNbt(rootTag);
        }
    }

    public void savePlayerToFile(Player player) {
        #if UNITY_EDITOR
            if(this.dontWriteToDisk) { return; }
        #endif

        NbtCompound rootTag = new NbtCompound("player");
        player.writeToNbt(rootTag);

        NbtFile file = new NbtFile(rootTag);
        file.SaveToFile(this.getPlayerFileName(player), NbtCompression.None);
    }

    /// <summary>
    /// Returns the name of the file that this map should be saved to.
    /// </summary>
    private string getSaveFileName() {
        return this.saveFolder + "/map" + GameSaver.EXTENSION;
    }

    private string getMapDataFileName() {
        return this.saveFolder + "/mapdata" + GameSaver.EXTENSION;
    }

    private string getPlayerFileName(Player player) {
        return this.saveFolder + "/" + player.getTeam().getTeamName().ToLower() + GameSaver.EXTENSION;
    }

    /// <summary>
    /// Create the passed directory if it doesn't exists.
    /// </summary>
    private void makeDirIfMissing(DirectoryInfo directory) {
        if(!directory.Exists) { // && !this.dontWriteToDisk) {
            directory.Create();
        }
    }
}