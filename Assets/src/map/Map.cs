using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Map : NetworkBehaviour {

    public static Map instance;

    private int timer;
    private EnumGameState gameState;

    public List<MapObject> mapObjects;
    public MessageProcesser processer;

    public void Start() {
        Map.instance = this;
    }

    public override void OnStartServer() {
        if (this.isServer) {
            this.mapObjects = new List<MapObject>();
        }

        this.processer = new MessageProcesser(this);
        this.processer.registerHandlers();
    }

    private void Update() {
        if(this.isServer) {
            for (int i = this.mapObjects.Count - 1; i >= 0; i--) {
                this.mapObjects[i].onUpdate();
            }
        }
    }

    public MapObject spawnEntity(GameObject prefab, Vector3 position, Quaternion? rotation) {
        if(rotation == null) {
            rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
        }

        GameObject enemy = GameObject.Instantiate(prefab, position, (Quaternion)rotation);
        NetworkServer.Spawn(enemy);

        MapObject mapObject = enemy.GetComponent<MapObject>();
        this.mapObjects.Add(mapObject);

        return mapObject;
    }

    public void removeEntity(MapObject mapObj, bool haveDeathEffect = true) {
        this.mapObjects.Remove(mapObj);

        if (haveDeathEffect) {
            if(mapObj is LivingObject) {
                ((LivingObject)mapObj).onDeath();
            }
        }

        GameObject.Destroy(mapObj.gameObject);
    }

    public void sendMessage(NetworkClient client, MessageBase message, short messageID) {
        client.Send(messageID, message);
    }
}
