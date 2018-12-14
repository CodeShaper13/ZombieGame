using UnityEngine;
using System;

public class RegisteredObject {

    private readonly GameObject prefab;
    private readonly int id;
    private readonly Type type;

    public RegisteredObject(int id, GameObject prefab) {
        this.id = id;
        if (prefab == null) {
            Application.Quit();
            throw new Exception("An object was registered with an id of " + id + " but the prefab was null!  Stopping Application!");
        }
        else {
            this.prefab = prefab;
            MapObject mapObj = this.prefab.GetComponent<MapObject>();
            if (mapObj == null) {
                Debug.Log(this.prefab.name);
                Application.Quit();
                throw new Exception("An object was registered with an id of " + id + " but the prefab contains no components of type MapObject!  Stopping Application!");
            }
            this.type = mapObj.GetType();
        }
    }

    public GameObject getPrefab() {
        return this.prefab;
    }

    public int getId() {
        return this.id;
    }

    public Type getType() {
        return this.type;
    }
}
