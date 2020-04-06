/// <summary>
/// Constants that can be easily changed to help with game balancing.
/// </summary>
public static class Constants {

    public static KeyedSettings ks;

    #region General:
    public static int STARTING_RESOURCES;
    public static int STARTING_TROOP_CAP;
    #endregion

    #region AI:
    public static float AI_MELEE_ATTACK_RATE; // Seconds between attacks.
    public static float AI_RANGE_ATTACK_RATE;

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
    public static int BUILDING_BRIDGE_HEALTH;
    public static int BUILDING_BRIDGE_COST;

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

    #region Structs
    public static BuildingData BD_CAMP;
    public static BuildingData BD_PRODUCER;
    public static BuildingData BD_WORKSHOP;
    public static BuildingData BD_TRAINING_HOUSE;
    public static BuildingData BD_STOREROOM;
    public static BuildingData BD_TOWER;
    public static BuildingData BD_WALL;
    public static BuildingData BD_FLAG;
    public static BuildingData BD_BRIDGE;
    #endregion

    public static void bootstrap() {
        Constants.ks = new KeyedSettings("gameData", "settings.txt", true);

        STARTING_RESOURCES = ks.getInt("GENERAL_STARTING-RESOURCES", 100, "The number of resources that the player starts with.");
        STARTING_TROOP_CAP = ks.getInt("GENERAL_STARTING-TROOP-CAP", 6);

        // Camp
        BUILDING_CAMP_HEALTH = ks.getInt("BUILDING_CAMP_HEALTH", 100);
        BUILDING_CAMP_COST = ks.getInt("BUILDING_CAMP_COST", 250);
        BUILDING_CAMP_TROOP_BOOST = ks.getInt("BUILDING_CAMP_TROOP-BOOST", 4);

        // Producer
        BUILDING_PRODUCER_HEALTH = ks.getInt("BUILDING_PRODUCER_HEALTH", 100);
        BUILDING_PRODUCER_COST = ks.getInt("BUILDING_PRODUCER_COST", 150);
        BUILDING_PRODUCER_RATE = ks.getFloat("BUILDING_PRODUCER_PRODUCE-RATE", 3.0f, "How often in seconds this building produces one resource.");
        BUILDING_PRODUCER_MAX_HOLD = ks.getInt("BUILDING_PRODUCER_MAX-HOLD", 100);

        // Training House
        BUILDING_TRAINING_HEALTH = ks.getInt("BUILDING_TRAINING-HOUSE_HEALTH", 100);
        BUILDING_TRAINING_COST = ks.getInt("BUILDING_TRAINING-HOUSE_COST", 300);
        BUILDING_TRAINING_HOUSE_QUEUE_SIZE = ks.getInt("BULDING_TRAINER_QUEUE-SIZE", 3);

        // Storeroom
        BUILDING_STOREROOM_HEALTH = ks.getInt("BUILDING_STOREROOM_HEALTH", 250);
        BUILDING_STOREROOM_COST = ks.getInt("BUILDING_STOREROOM_COST", 500);
        BUILDING_STOREROOM_RESOURCE_BOOST = ks.getInt("BUILDING_STOREROOM_RESOURCE-BOOST", 300);

        // Tower
        BUILDING_TOWER_HEALTH = ks.getInt("BUILDING_TOWER_HEALTH", 250);
        BUILDING_TOWER_COST = ks.getInt("BUILDING_TOWER_COST", 600);
        BUILDING_TOWER_FIRE_SPEED = ks.getFloat("BUILDING_TOWER_FIRE-SPEED", 2f, "How often in seconds the tower fires a bullet");
        BUILDING_TOWER_DAMAGE = ks.getInt("BUILDING_TOWER_DAMAGE", 20);
        BUILDING_TOWER_FIRE_RANGE = ks.getFloat("BUILDING_TOWER_FIRE-RANGE", 20f);
        BUILDING_TOWER_SEE_RANGE = ks.getFloat("BUILDING_TOWER_SEE-RANGE", 20f, "How far away a target must be for the tower to start shooting at it.");

        // Flag
        BUILDING_FLAG_HEALTH = ks.getInt("BUILDING_FLAG_HEALTH", 25);
        BUILDING_FLAG_COST = ks.getInt("BUILDING_FLAG_COST", 100);

        // Bridge
        BUILDING_BRIDGE_HEALTH = ks.getInt("BUILDING_BRIDGE_HEALTH", 25);
        BUILDING_BRIDGE_COST = ks.getInt("BUILDING_BRIDGE_COST", 100);

        // Workshop (UNUSED)
        BUILDING_WORKSHOP_HEALTH = ks.getInt("BUILDING_WORKSHOP_HEALTH", 100, "(UNUSED)"); // Not yet implemented
        BUILDING_WORKSHOP_COST = ks.getInt("BUILDING_WORKSHOP_COST", 100, "(UNUSED)"); // Not yet implemented
        BUILDING_WORKSHOP_QUEUE_SIZE = ks.getInt("BUILDING_WORKSHOP_QUEUE-SIZE", 2, "(UNUSED)"); // Not yet implemented

        AI_MELEE_ATTACK_RATE = ks.getFloat("AI_TROOP_MELEE-ATTACK-RATE", 1f, "Seconds between attacks.");
        AI_RANGE_ATTACK_RATE = ks.getFloat("AI_TROOP_RANGE-ATTACK-RATE", 1f, "Seconds between attacks.");

        AI_FIGHTING_FIND_RANGE = ks.getFloat("AI_FIGHTING_FIND-RANGE", 5f, "How far away units will see another troop.");
        AI_FIGHTING_DEFEND_RANGE = ks.getFloat("AI_FIGHTING_DEFEND-RANGE", 3f, "The distance a unit will move to defend a point.");

        // Soldier

        // Archer
        AI_ARCHER_SHOOT_RANGE = ks.getFloat("AI_ARCHER_SHOOT-RANGE", 4f);
        AI_ARCHER_STOP_RANGE = ks.getFloat("AI_ARCHER_STOP-RANGE", 5f);

        // Builder

        ks.save();

        // Create the sturctures
        BD_CAMP = new BuildingData("Camp", BUILDING_CAMP_HEALTH, BUILDING_CAMP_COST);
        BD_PRODUCER = new BuildingData("Producer", BUILDING_PRODUCER_HEALTH, BUILDING_PRODUCER_COST);
        BD_WORKSHOP = new BuildingData("Workshop", BUILDING_WORKSHOP_HEALTH, BUILDING_WORKSHOP_COST);
        BD_TRAINING_HOUSE = new BuildingData("Training House", BUILDING_TRAINING_HEALTH, BUILDING_TRAINING_COST);
        BD_STOREROOM = new BuildingData("Store Room", BUILDING_STOREROOM_HEALTH, BUILDING_STOREROOM_COST);
        BD_TOWER = new BuildingData("Tower", BUILDING_TOWER_HEALTH, BUILDING_TOWER_COST);
        BD_WALL = new BuildingData("Wall", 250, 100, true);
        BD_FLAG = new BuildingData("Flag", BUILDING_FLAG_HEALTH, BUILDING_FLAG_COST);
        BD_BRIDGE = new BuildingData("Bridge", BUILDING_BRIDGE_HEALTH, BUILDING_BRIDGE_COST);
    }
}