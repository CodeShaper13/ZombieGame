using UnityEngine;

public abstract class MapGenerator {

    protected Map map;
    private System.Random random;

    public MapGenerator(Map map, int seed) {
        this.map = map;
        this.random = new System.Random(seed);
    }

    public abstract void generateMap(MapData mapData);

    protected void spawnRock(float x, float z, bool calculateElevation = false) {
        const float f1 = 2f;
        const float f2 = 3f;
        Vector3 scale = new Vector3(this.rndFloat(f1, f2), this.rndFloat(f1, f2), this.rndFloat(f1, f2));
        this.map.spawnEntity<MapObject>(Registry.harvestableRock, new Vector3(x, calculateElevation ? this.getElevation(x, z) : 0, z), this.getRndRot(), scale);
    }

    /// <summary>
    /// Spawns a tree with a random size on the map.
    /// </summary>
    protected void spawnTree(float x, float z, bool calculateElevation = false) {
        float f = this.rndFloat(0.6f, 1.6f);
        Vector3 scale = new Vector3(f, f + (f * this.rndFloat(-0.2f, 0.2f)), f);
        this.map.spawnEntity<MapObject>(Registry.harvestableTree, new Vector3(x, calculateElevation ? this.getElevation(x, z) : 0, z), this.getRndRot(), scale);
    }

    /// <summary>
    /// Returns a random rotation on the y axis.  The seed does NOT influence this.
    /// </summary>
    protected Quaternion getRndRot() {
        return Quaternion.Euler(0, Random.Range(0, 359), 0);
    }

    protected int rndInt(int min, int max) {
        return this.random.Next(min, max);
    }

    protected float rndFloat(float min, float max) {
        return (float)this.random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Returns the elevation of the terrain at the passed point.  0 is returned if there is no terrain at the passed position.
    /// </summary>
    protected float getElevation(float x, float z) {
        RaycastHit hit;
        if(Physics.Raycast(new Vector3(x, 100, z), Vector3.down, out hit, 200, Layers.GROUND)) {
            return hit.point.y;
        }
        return 0;
    }

    /// <summary>
    /// Creates a MapGenerator from the passed enum.
    /// </summary>
    public static MapGenerator getGeneratorFromEnum(EnumGeneratorType type, Map map, int seed) {
        switch(type) {
            case EnumGeneratorType.FOREST:
                return new MapGeneratorForest(map, seed);
            case EnumGeneratorType.PLAINS:
                return new MapGeneratorPlains(map, seed);
            case EnumGeneratorType.MOUNTAINS:
                return new MapGeneratorMountain(map, seed);
        }
        return null;
    }
}
