using fNbt;
using System.IO;

public class GameSaver {

    /// <summary> The extension of the save files. </summary>
    private const string EXTENSION = ".nbt";

    private bool dontWriteToDisk = false;

    private string saveName;
    private string saveFolderName;
    private string playerFolderName;

    public GameSaver(string saveName) {
        this.saveName = saveName;

        this.saveFolderName = "saves/" + this.saveName + "/";
        this.playerFolderName = this.saveFolderName + "players/";

        this.makeDirIfMissing(this.saveFolderName);
        this.makeDirIfMissing(this.playerFolderName);
    }

    /// <summary>
    /// Returns true if this save can be found on the disk.
    /// </summary>
    public bool doesSaveExists() {
        return File.Exists(this.getSaveFileName());
    }

    /// <summary>
    /// Returns true if a save file exists for the passed Player.
    /// </summary>
    public bool doesPlayerSaveExist(Player player) {
        return File.Exists(this.getPlayerFileName(player));
    }

    public void readMapFromFile(Map map) {
        string s = this.getSaveFileName();
        if(File.Exists(s)) {
            NbtFile file = new NbtFile();
            file.LoadFromFile(s);

            NbtCompound rootTag = file.RootTag;
            map.readFromNbt(rootTag);
        }
    }

    public void saveMapToFile(Map map) {
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
            if(this.dontWriteToDisk) {
                return;
            }
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
        return this.saveFolderName + "map" + GameSaver.EXTENSION;
    }

    private string getPlayerFileName(Player player) {
        return this.playerFolderName + player.getTeam().getTeamName().ToLower() + GameSaver.EXTENSION;
    }

    /// <summary>
    /// Create the passed directory if it doesn't exists.
    /// </summary>
    private void makeDirIfMissing(string name) {
        if(!this.dontWriteToDisk && !Directory.Exists(name)) {
            Directory.CreateDirectory(name);
        }
    }
}