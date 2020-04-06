using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
/// A list of all the Prefabs that will be spawned.  These
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
    public GameObject prefabUnitMedic;
    public GameObject prefabZombie;

    [Header("Projectiles")]
    public GameObject prefabProjectileBullet;

    [Header("Buildings")]
    public GameObject prefabBuildingCamp;
    public GameObject prefabBuildingCannon;
    public GameObject prefabBuildingFlag;
    public GameObject prefabBuildingProducer;
    public GameObject prefabBuildingStoreroom;
    public GameObject prefabBuildingTrainingHouse;
    public GameObject prefabBuildingWorkshop;
    public GameObject prefabBuildingBridge;

    [Header("Harvestables")]
    //public GameObject prefabHarvestableRock;

    [Header("UI")] // (Not registered)
    public GameObject prefabPlaceObjectButton;

    public GameObject prefabGameModeTimer;

    [Header("")]

    // Other:
    public GameObject prefabHealthBarEffect;

    [Header("GUI Objects")]
    public GuiBase guiPausedObject;
    public GuiBase guiUnitStatsObject;
    public GuiBase guiCampaignSelect;
    public GuiBase guiCampaignWin;

    private void Awake() {
        References.list = this;

        this.add(this.prefabUnitSoldier);
        this.add(this.prefabUnitArcher);
        this.add(this.prefabUnitBuilder);
        //this.add(this.prefabUnitMedic);
        this.add(this.prefabZombie);

        this.add(this.prefabProjectileBullet);

        this.add(this.prefabBuildingCamp);
        this.add(this.prefabBuildingCannon);
        this.add(this.prefabBuildingFlag);
        this.add(this.prefabBuildingProducer);
        this.add(this.prefabBuildingStoreroom);
        this.add(this.prefabBuildingTrainingHouse);
        this.add(this.prefabBuildingWorkshop);
        this.add(this.prefabBuildingBridge);

        this.add(this.prefabGameModeTimer);
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
