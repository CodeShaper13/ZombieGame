using fNbt;

public class MapData {

    public string saveName;
    public int seed;

    public MapData() : this("nul", 0) { }

    public MapData(string name, int seed) {
        this.saveName = name;
        this.seed = seed;
    }

    /// <summary>
    /// Creates a new map data from NBT.
    /// </summary>
    public MapData(NbtCompound tag) {
        this.saveName = tag.getString("mapName");
        this.seed = tag.getInt("seed");
    }

    public NbtCompound writeToNbt() {
        NbtCompound tag = new NbtCompound("data");

        tag.setTag("mapName", this.saveName);
        tag.setTag("seed", this.seed);

        return tag;
    }
}
