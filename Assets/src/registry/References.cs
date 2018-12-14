using UnityEngine;
using System.Collections;
using System;
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

    [SerializeField]
    private NetworkManager networkManager;

    [Header("")] // Add an empty line.

    // Objects that are registered:
    public GameObject prefabUnitSoldier;
    public GameObject prefabProjectileBullet;
    public GameObject prefabBuildingFlag;
    public GameObject prefabBuildingWall;
    public GameObject prefabBuildingCannon;

    // Other:
    public GameObject prefabHealthBarEffect;

    // UI: (Not registered)
    public GameObject prefabPlaceObjectButton;

    // Unused:
    public GameObject prefabEnemy;

    private void Awake() {
        References.list = this;

        this.add(this.prefabUnitSoldier);

        this.add(this.prefabProjectileBullet);

        this.add(this.prefabBuildingFlag);
        this.add(this.prefabBuildingWall);
        this.add(this.prefabBuildingCannon);

        this.add(this.prefabHealthBarEffect);

        this.add(this.prefabEnemy);

        foreach (GameObject prefab in this.allPrefabs) {
            networkManager.spawnPrefabs.Add(prefab);
        }
    }

    private void add(GameObject prefab) {
        if(prefab != null) {
            this.allPrefabs.Add(prefab);
        }
    }
}
