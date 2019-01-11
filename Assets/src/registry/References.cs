using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
/// A list of all the Prefabs that will be spawned.  Theses
/// Prefabs are registered to the Network manager to be spawned
/// over the network.
/// </summary>
public class References : MonoBehaviour {

    public static References list;

    private List<GameObject> allPrefabs = new List<GameObject>();

    [Header("Units")]
    public GameObject prefabUnitSoldier;
    public GameObject prefabUnitArcher;
    public GameObject prefabUnitBuilder;

    [Header("Projectiles")]
    public GameObject prefabProjectileArrow;

    [Header("Buildings")]
    public GameObject prefabBuildingCamp;
    public GameObject prefabBuildingCannon;
    public GameObject prefabBuildingFlag;
    public GameObject prefabBuildingProducer;
    public GameObject prefabBuildingStoreroom;
    public GameObject prefabBuildingTrainingHouse;
    public GameObject prefabBuildingWorkshop;
    public GameObject prefabBuildingWall;

    [Header("Harvestables")]
    public GameObject prefabHarvestableTree;
    public GameObject prefabHarvestableRock;

    [Header("UI")] // (Not registered)
    public GameObject prefabPlaceObjectButton;

    [Header("Text")]
    public TextAsset maleNames;
    public TextAsset femaleNames;
    public TextAsset lastNames;
    public TextAsset constants;

    [Header("Player")] // (Not registered)
    public GameObject prefabUnitDestinationEffect;
    public GameObject prefabAttackTargetEffect;

    [Header("")]

    public GameObject mapPrefab;

    // Other:
    public GameObject prefabHealthBarEffect;

    [Header("GUI Objects")]
    public GuiBase guiPausedObject;
    public GuiBase guiUnitStatsObject;

    private void Awake() {
        References.list = this;

        this.add(this.prefabUnitSoldier);
        this.add(this.prefabUnitArcher);
        this.add(this.prefabUnitBuilder);

        this.add(this.prefabProjectileArrow);

        this.add(this.prefabBuildingCamp);
        this.add(this.prefabBuildingCannon);
        this.add(this.prefabBuildingFlag);
        this.add(this.prefabBuildingProducer);
        this.add(this.prefabBuildingStoreroom);
        this.add(this.prefabBuildingTrainingHouse);
        this.add(this.prefabBuildingWorkshop);
        this.add(this.prefabBuildingWall);

        this.add(this.prefabHarvestableTree);
        this.add(this.prefabHarvestableRock);

        this.add(this.prefabHealthBarEffect);

        this.add(this.mapPrefab);
    }

    public void registerPrefabsToNetworkManager(NetworkManager networkManager) {
        foreach (GameObject prefab in this.allPrefabs) {
            networkManager.spawnPrefabs.Add(prefab);
        }
    }

    private void add(GameObject prefab) {
        if(prefab == null) {
            Debug.LogWarning("Tried to register null prefab, this is not good!");
            return;
        }
        if(this.allPrefabs.Contains(prefab)) {
            Debug.LogWarning("Duplicate prefabs with the name \"" + prefab.name + "\" registered, this is not good!");
        }
        this.allPrefabs.Add(prefab);
    }
}
