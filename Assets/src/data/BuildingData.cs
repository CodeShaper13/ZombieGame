public struct BuildingData {

    private readonly string name;
    private readonly int maxHealth;
    private readonly int cost;
    private readonly bool instantBuild;

    public BuildingData(string buildingName, int maxHealth, int cost) : this(buildingName, maxHealth, cost, false) { }

    public BuildingData(string buildingName, int maxHealth, int cost, bool instantBuild) {
        this.name = buildingName;
        this.maxHealth = maxHealth;
        this.cost = cost;
        this.instantBuild = instantBuild;
    }

    public string getName() {
        return this.name;
    }

    public int getMaxHealth() {
        return this.maxHealth;
    }

    public int getCost() {
        return this.cost;
    }

    public bool isInstantBuild() {
        return this.instantBuild;
    }
}