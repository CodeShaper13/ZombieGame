using UnityEngine;

// Unused!!!

[DisallowMultipleComponent]
public class SinglePlayerInit : MonoBehaviour {

    [SerializeField]
    private MapBase map;

    private void Awake() {
        print("!!!");
        //NetworkManager.singleton.

        this.map.gameObject.SetActive(true);
    }

    private void Start() {
        this.map.initialize(new MapData("world", 123));
    }

    private void OnEnable() {
        print("onenable");
    }
}

