using System.IO;
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
    public static readonly EntityBaseStats ED_BUILDER = new EntityBaseStats("Builder", 35, 3.5f, 5, 10, 25, 45f);
    public static readonly EntityBaseStats ED_SOLDIER = new EntityBaseStats("Soldier", 75, 3.5f, 5, 10, 25, 1f);
    public static readonly EntityBaseStats ED_ARCHER = new EntityBaseStats("Archer", 50, 3.5f, 5, 10, 25, 15f);
    public static readonly EntityBaseStats ED_HEAVY = new EntityBaseStats("Heavy", 100, 3.5f, 5, 10, 25, 25f);

    public static readonly EntityBaseStats ED_WAR_WAGON = new EntityBaseStats("War Wagon", 500, 3.5f, 3, 10, 100, 60f);
    public static readonly EntityBaseStats ED_CANNON = new EntityBaseStats("Cannon", 150, 3.5f, 200, 10, 100, 45f);

    // Only a float to prevent casting in math operations.
    public static float BASE_DEFENSE_VALUE = 10;

    // Builder
    public static int BUILDER_MAX_CARRY;
    public static int BUILDER_COLLECT_PER_STRIKE;
    public static float BUILDER_STRIKE_RATE;
    #endregion

    #region AI:
    public static float AI_MELEE_ATTACK_RATE; // Seconds between attacks.

    public static float AI_FIGHTING_FIND_RANGE;
    public static float AI_FIGHTING_DEFEND_RANGE;

    public static float AI_ARCHER_SHOOT_RANGE;
    public static float AI_ARCHER_STOP_RANGE;
    #endregion

    #region Buildings:
    public static readonly BuildingData BD_CAMP = new BuildingData("Camp", 250, 100);
    public static readonly BuildingData BD_PRODUCER = new BuildingData("Producer", 250, 200);
    public static readonly BuildingData BD_WORKSHOP = new BuildingData("Workshop", 250, 350);
    public static readonly BuildingData BD_TRAINING_HOUSE = new BuildingData("Training House", 250, 200);
    public static readonly BuildingData BD_STOREROOM = new BuildingData("Store Room", 250, 250);
    public static readonly BuildingData BD_TOWER = new BuildingData("Tower", 250, 250);
    public static readonly BuildingData BD_WALL = new BuildingData("Wall", 500, 50, true);
    public static readonly BuildingData BD_FLAG = new BuildingData("Flag", 100, 10);

    public static int BUILDING_TRAINING_HOUSE_QUEUE_SIZE;
    public static int BUILDING_WORKSHOP_QUEUE_SIZE;

    public static int CONSTRUCT_RATE;

    public static int BUILDING_CAMP_TROOP_BOOST;

    public static int BUILDING_STOREROOM_RESOURCE_BOOST;

    public static float BUILDING_PRODUCER_RATE;
    public static int BUILDING_PRODUCER_MAX_HOLD;

    public static float BUILDING_TOWER_FIRE_SPEED;
    public static float BUILDING_TOWER_FIRE_RANGE;
    public static int BUILDING_TOWER_DAMAGE;
    public static float BUILDING_TOWER_SEE_RANGE;
    #endregion


    public static void bootstrap() {
        KeyedSettings ks = new KeyedSettings(References.list.constants, true);

        STARTING_RESOURCES = ks.getInt("GENERAL_STARTING-RESOURCES", 1000, "The number of resources that the player starts with.");
        STARTING_TROOP_CAP = ks.getInt("GENERAL_STARTING-TROOP-CAP", 6);
        STARTING_RESOURCE_CAP = ks.getInt("GENERAL_STARTING-RESOURCE-CAP", 500);

        CONSTRUCT_RATE = ks.getInt("BUILDING-CONSTRUCT-RATE", 10);

        BUILDING_CAMP_TROOP_BOOST = ks.getInt("BUILDING_CAMP_TROOP-BOOST", 4);

        BUILDING_STOREROOM_RESOURCE_BOOST = ks.getInt("BUILDING_STOREROOM_RESOURCE-BOOST", 250);

        BUILDING_PRODUCER_RATE = ks.getFloat("BUILDING_PRODUCER_PRODUCE-RATE", 2f, "How often in seconds this building produces one resource.");
        BUILDING_PRODUCER_MAX_HOLD = ks.getInt("BUILDING_PRODUCER_MAX-HOLD", 100);

        BUILDING_TOWER_FIRE_SPEED = ks.getFloat("BUILDING_TOWER_FIRE-SPEED", 1.5f);
        BUILDING_TOWER_FIRE_RANGE = ks.getFloat("BUILDING_TOWER_FIRE-RANGE", 20f);
        BUILDING_TOWER_DAMAGE = ks.getInt("BUILDING_TOWER_DAMAGE", 15);
        BUILDING_TOWER_SEE_RANGE = ks.getFloat("BUILDING_TOWER_SEE-RANGE", 10f);

        BUILDING_TRAINING_HOUSE_QUEUE_SIZE = ks.getInt("BULDING_TRAINER_QUEUE-SIZE", 3);
        BUILDING_WORKSHOP_QUEUE_SIZE = ks.getInt("BUILDING_WORKSHOP_QUEUE-SIZE", 2);

        AI_MELEE_ATTACK_RATE = ks.getFloat("AI_TROOP_ATTACK-RATE", 1f, "Seconds between attacks.");

        AI_FIGHTING_FIND_RANGE = ks.getFloat("AI_FIGHTING_FIND-RANGE", 30f);
        AI_FIGHTING_DEFEND_RANGE = ks.getFloat("AI_FIGHTING_DEFEND-RANGE", 10f);

        AI_ARCHER_SHOOT_RANGE = ks.getFloat("AI_ARCHER_SHOOT-RANGE", 15f);
        AI_ARCHER_STOP_RANGE = ks.getFloat("AI_ARCHER_STOP-RANGE", 10f);

        BUILDER_MAX_CARRY = ks.getInt("UNIT_BUILDER_MAX-CARRY", 500, "How many resources a builder can carry with them.");
        BUILDER_COLLECT_PER_STRIKE = ks.getInt("UNIT_BUILDER_COLLECT-PER-STRIKE", 5, "How many resources are collected/damage done every time a builder hits a harvestable.");
        BUILDER_STRIKE_RATE = ks.getFloat("UNIT_BUILDER_STRIKE-RATE", 1f);

        ks.save("Assets/ZombieGame/data/settings.txt");
    }
}
