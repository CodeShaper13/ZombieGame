using System;
using UnityEngine;

public class Registry {

    private const int REGISTRY_SIZE = 255;

    public static RegisteredObject unitSoldier;
    public static RegisteredObject unitArcher;
    public static RegisteredObject unitScout;

    public static RegisteredObject projectileBullet;

    public static RegisteredObject buildingFlag;
    public static RegisteredObject buildingWall;
    public static RegisteredObject buildingCannon;

    /// <summary> If true, the registry has been initialized. </summary>
    private static bool ranBootstrap;
    private static RegisteredObject[] objectRegistry;

    public Registry() {
        Registry.objectRegistry = new RegisteredObject[REGISTRY_SIZE];
    }

    /// <summary>
    /// Returns a RegisteredObject from the registry by ID, or null if nothing is registered with that id.
    /// </summary>
    public static RegisteredObject getObjectFromRegistry(int id) {
        if (id < 0 || id > REGISTRY_SIZE) {
            throw new Exception("Index out of registry range!");
        }
        return Registry.objectRegistry[id];
    }

    /// <summary>
    /// Returns the ID of the passed Entity, or -1 on error.
    /// </summary>
    public static int getIdFromObject(MapObject entity) {
        Type t = entity.GetType();
        RegisteredObject re;
        for (int i = 0; i < REGISTRY_SIZE; i++) {
            re = Registry.objectRegistry[i];
            if (re != null && t == re.getType()) {
                return re.getId();
            }
        }
        return -1;
    }

    /// <summary>
    /// Initializes the registries if they haven't already been initialized.
    /// </summary>
    public static void registryBootstrap() {
        if (!Registry.ranBootstrap) {
            new Registry().registerAllObjects();
            Registry.ranBootstrap = true;
        }
    }

    private Registry registerAllObjects() {
        // dont register anything with an id of 0, this is an error id.

        // Units are ids 1 - 63.
        Registry.unitSoldier = register(1, References.list.prefabUnitSoldier);

        // Harvestable ids are 64 - 95.

        // Projectile ids are 96-127.
        Registry.projectileBullet = register(96, References.list.prefabProjectileBullet);

        // Buildings are ids 128 - 255.
        Registry.buildingFlag = register(128, References.list.prefabBuildingFlag);
        Registry.buildingWall = register(129, References.list.prefabBuildingWall);
//        Registry.buildingCannon = register(130, References.list.prefabBuildingCannon);

        return this;
    }

    private RegisteredObject register(int id, GameObject prefab) {
        RegisteredObject registerObject = new RegisteredObject(id, prefab);

        int i = registerObject.getId();
        if (Registry.objectRegistry[i] != null) {
            throw new Exception("Two objects were registered with the same ID!");
        }
        Registry.objectRegistry[i] = registerObject;
        return registerObject;
    }

    private RegisteredObject register(int id, string prefabName) {
        return this.register(id, Resources.Load<GameObject>(prefabName));
    }
}
