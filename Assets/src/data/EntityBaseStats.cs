public struct EntityBaseStats {

    private readonly string unitTypeName;
    public readonly int baseHealth;
    public readonly float baseSpeedMultiplyer;
    public readonly int baseAttack;
    public readonly int baseDefense;
    public readonly int cost;
    private readonly float productionTime;

    public EntityBaseStats(string name, int health, int attack, int defense, float speed, int cost, float productionTime) {
        this.unitTypeName = name;
        this.baseHealth = health;
        this.baseAttack = attack;
        this.baseDefense = defense;
        this.baseSpeedMultiplyer = speed;
        this.cost = cost;
        this.productionTime = productionTime;
    }

    public string getUnitTypeName() {
        return this.unitTypeName;
    }

    public float getProductionTime() {
        return this.productionTime;
    }
}