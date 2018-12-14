using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public int numberOfEnemies;

    public override void OnStartServer() {
        for (int i = 0; i < numberOfEnemies; i++) {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-8.0f, 8.0f),
                1f,
                Random.Range(-8.0f, 8.0f));

            Quaternion spawnRotation = Quaternion.Euler(
                0.0f,
                Random.Range(0, 180),
                0.0f);

            //GameObject prefab = Registry.list.enemy.prefab();
            //GameObject enemy = GameObject.Instantiate(prefab, spawnPosition, spawnRotation);
            //NetworkServer.Spawn(enemy);
        }
    }
}