/// <summary>
/// Constants that can be easily changed to help with game balancing.
/// </summary>
public static class Constants {

    #region General:
    public static int STARTING_RESOURCES;
    public static int STARTING_TROOP_CAP;
    public static int STARTING_RESOURCE_CAP;
    #endregion

    #region Units:
    public static int UNIT_HP_MINUS = 40;
    public static int UNIT_HP_MEDIAN = 50;
    public static int UNIT_HP_PLUS = 60;

    public static int UNIT_ATK_MINUS = 8;
    public static int UNIT_ATK_MEDIAN = 10;
    public static int UNIT_ATK_PLUS = 12;

    public static int UNIT_DEF_MINUS = -4;
    public static int UNIT_DEF_MEDIAN = 0;
    public static int UNIT_DEF_PLUS = 4;

    public static readonly EntityBaseStats ED_SOLDIER = new EntityBaseStats("Settler",
        UNIT_HP_MINUS,
        UNIT_ATK_PLUS,
        UNIT_DEF_MEDIAN,
        1,
        25, 15f);
    public static readonly EntityBaseStats ED_ARCHER = new EntityBaseStats("Soldier",
        UNIT_HP_PLUS,
        UNIT_ATK_PLUS,
        UNIT_DEF_MINUS,
        1,
        25, 15f);
    public static readonly EntityBaseStats ED_HEAVY = new EntityBaseStats("Miner",
        UNIT_HP_PLUS,
        UNIT_ATK_MEDIAN,
        UNIT_DEF_PLUS,
        1,
        25, 15f);
    public static readonly EntityBaseStats ED_BUILDER = new EntityBaseStats("Craftsman",
        UNIT_HP_MEDIAN,
        UNIT_ATK_MINUS,
        UNIT_DEF_PLUS,
        1,
        50, 30f);

    // Only a float to prevent casting in math operations.
    public static float BASE_DEFENSE_VALUE = 10;

    // Builder
    public static int BUILDER_MAX_CARRY;
    public static int BUILDER_COLLECT_PER_STRIKE;
    public static float BUILDER_STRIKE_RATE;
    public static int BUILDER_CONSTRUCT_RATE;

    #endregion

    #region AI:
    public static float AI_MELEE_ATTACK_RATE; // Seconds between attacks.

    public static float AI_FIGHTING_FIND_RANGE;
    public static float AI_FIGHTING_DEFEND_RANGE;

    public static float AI_ARCHER_SHOOT_RANGE;
    public static float AI_ARCHER_STOP_RANGE;
    #endregion

    #region Buildings:
    public static int BUILDING_CAMP_HEALTH;
    public static int BUILDING_CAMP_COST;
    public static int BUILDING_PRODUCER_HEALTH;
    public static int BUILDING_PRODUCER_COST;
    public static int BUILDING_WORKSHOP_HEALTH;
    public static int BUILDING_WORKSHOP_COST;
    public static int BUILDING_TRAINING_HEALTH;
    public static int BUILDING_TRAINING_COST;
    public static int BUILDING_STOREROOM_HEALTH;
    public static int BUILDING_STOREROOM_COST;
    public static int BUILDING_TOWER_HEALTH;
    public static int BUILDING_TOWER_COST;
    public static int BUILDING_FLAG_HEALTH;
    public static int BUILDING_FLAG_COST;

    public static int BUILDING_TRAINING_HOUSE_QUEUE_SIZE;
    public static int BUILDING_WORKSHOP_QUEUE_SIZE;

    public static int BUILDING_CAMP_TROOP_BOOST;

    public static int BUILDING_STOREROOM_RESOURCE_BOOST;
    public static int BUILDING_STOREROOM_MAX_HOLD = 500;

    public static float BUILDING_PRODUCER_RATE;
    public static int BUILDING_PRODUCER_MAX_HOLD;

    public static float BUILDING_TOWER_FIRE_SPEED;
    public static float BUILDING_TOWER_FIRE_RANGE;
    public static int BUILDING_TOWER_DAMAGE;
    public static float BUILDING_TOWER_SEE_RANGE;
    #endregion

    #region Harvestables:
    public static int HARVESTABLE_TREE_HEALTH;
    public static int HARVESTABLE_ROCK_HEALTH;
    #endregion

    #region Structs
    public static BuildingData BD_CAMP;
    public static BuildingData BD_PRODUCER;
    public static BuildingData BD_WORKSHOP;
    public static BuildingData BD_TRAINING_HOUSE;
    public static BuildingData BD_STOREROOM;
    public static BuildingData BD_TOWER;
    public static BuildingData BD_WALL;
    public static BuildingData BD_FLAG;
    #endregion

    public static void bootstrap() {
        KeyedSettings ks = new KeyedSettings(References.list.constants, true);

        STARTING_RESOURCES = ks.getInt("GENERAL_STARTING-RESOURCES", 0, "The number of resources that the player starts with.");
        STARTING_TROOP_CAP = ks.getInt("GENERAL_STARTING-TROOP-CAP", 6);
        STARTING_RESOURCE_CAP = ks.getInt("GENERAL_STARTING-RESOURCE-CAP", 500);

        BUILDING_CAMP_HEALTH = ks.getInt("BUILDING_CAMP_HEALTH", 100);
        BUILDING_CAMP_COST = ks.getInt("BUILDING_CAMP_COST", 250);
        BUILDING_CAMP_TROOP_BOOST = ks.getInt("BUILDING_CAMP_TROOP-BOOST", 4);

        BUILDING_PRODUCER_HEALTH = ks.getInt("BUILDING_PRODUCER_HEALTH", 100);
        BUILDING_PRODUCER_COST = ks.getInt("BUILDING_PRODUCER_COST", 150);
        BUILDING_PRODUCER_RATE = ks.getFloat("BUILDING_PRODUCER_PRODUCE-RATE", 1f, "How often in seconds this building produces one resource.");
        BUILDING_PRODUCER_MAX_HOLD = ks.getInt("BUILDING_PRODUCER_MAX-HOLD", 100);

        BUILDING_TRAINING_HEALTH = ks.getInt("BUILDING_TRAINING-HOUSE_HEALTH", 100);
        BUILDING_TRAINING_COST = ks.getInt("BUILDING_TRAINING-HOUSE_COST", 300);
        BUILDING_TRAINING_HOUSE_QUEUE_SIZE = ks.getInt("BULDING_TRAINER_QUEUE-SIZE", 3);

        BUILDING_STOREROOM_HEALTH = ks.getInt("BUILDING_STOREROOM_HEALTH", 250);
        BUILDING_STOREROOM_COST = ks.getInt("BUILDING_STOREROOM_COST", 500);
        BUILDING_STOREROOM_RESOURCE_BOOST = ks.getInt("BUILDING_STOREROOM_RESOURCE-BOOST", 250);

        BUILDING_TOWER_HEALTH = ks.getInt("BUILDING_TOWER_HEALTH", 100);
        BUILDING_TOWER_COST = ks.getInt("BUILDING_TOWER_COST", 400);
        BUILDING_TOWER_FIRE_SPEED = ks.getFloat("BUILDING_TOWER_FIRE-SPEED", 2f);
        BUILDING_TOWER_DAMAGE = ks.getInt("BUILDING_TOWER_DAMAGE", 20);
        BUILDING_TOWER_FIRE_RANGE = ks.getFloat("BUILDING_TOWER_FIRE-RANGE", 20f);
        BUILDING_TOWER_SEE_RANGE = ks.getFloat("BUILDING_TOWER_SEE-RANGE", 15f);

        BUILDING_FLAG_HEALTH = ks.getInt("BUILDING_FLAG_HEALTH", 25);
        BUILDING_FLAG_COST = ks.getInt("BUILDING_FLAG_COST", 50);

        BUILDING_WORKSHOP_HEALTH = ks.getInt("BUILDING_WORKSHOP_HEALTH", 100); // Not yet implemented
        BUILDING_WORKSHOP_COST = ks.getInt("BUILDING_WORKSHOP_COST", 100); // Not yet implemented
        BUILDING_WORKSHOP_QUEUE_SIZE = ks.getInt("BUILDING_WORKSHOP_QUEUE-SIZE", 2); // Not yet implemented

        AI_MELEE_ATTACK_RATE = ks.getFloat("AI_TROOP_ATTACK-RATE", 1f, "Seconds between attacks.");

        AI_FIGHTING_FIND_RANGE = ks.getFloat("AI_FIGHTING_FIND-RANGE", 30f);
        AI_FIGHTING_DEFEND_RANGE = ks.getFloat("AI_FIGHTING_DEFEND-RANGE", 10f);

        AI_ARCHER_SHOOT_RANGE = ks.getFloat("AI_ARCHER_SHOOT-RANGE", 15f);
        AI_ARCHER_STOP_RANGE = ks.getFloat("AI_ARCHER_STOP-RANGE", 10f);

        BUILDER_MAX_CARRY = ks.getInt("UNIT_BUILDER_MAX-CARRY", 250, "How many resources a builder can carry with them.");
        BUILDER_COLLECT_PER_STRIKE = ks.getInt("UNIT_BUILDER_COLLECT-PER-STRIKE", 7, "How many resources are collected/damage done every time a builder hits a harvestable.");
        BUILDER_STRIKE_RATE = ks.getFloat("UNIT_BUILDER_STRIKE-RATE", 1f, "How often in seconds the builder hits a building/harvestable while building/harvesting.");
        BUILDER_CONSTRUCT_RATE = ks.getInt("UNIT_BUILDER_CONSTRUCT-RATE", 10);

        HARVESTABLE_TREE_HEALTH = ks.getInt("HARVESTABLE_TREE-HEALTH", 40);
        HARVESTABLE_ROCK_HEALTH = ks.getInt("HARVESTABLE_ROCK-HEALTH", 55);

        ks.save("settings/", "settings.txt");

        // Create the sturctures
        BD_CAMP = new BuildingData("Camp", BUILDING_CAMP_HEALTH, BUILDING_CAMP_COST);
        BD_PRODUCER = new BuildingData("Producer", BUILDING_PRODUCER_HEALTH, BUILDING_PRODUCER_COST);
        BD_WORKSHOP = new BuildingData("Workshop", BUILDING_WORKSHOP_HEALTH, BUILDING_WORKSHOP_COST);
        BD_TRAINING_HOUSE = new BuildingData("Training House", BUILDING_TRAINING_HEALTH, BUILDING_TRAINING_COST);
        BD_STOREROOM = new BuildingData("Store Room", BUILDING_STOREROOM_HEALTH, BUILDING_STOREROOM_COST);
        BD_TOWER = new BuildingData("Tower", BUILDING_TOWER_HEALTH, BUILDING_TOWER_COST);
        BD_WALL = new BuildingData("Wall", 250, 100, true);
        BD_FLAG = new BuildingData("Flag", BUILDING_FLAG_HEALTH, BUILDING_FLAG_COST);
    }
}
