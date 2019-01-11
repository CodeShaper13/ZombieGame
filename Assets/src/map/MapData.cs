using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour {

    public int setupTime = 60;

    public EnumGeneratorType mapGenerator;

    [Header("")]

    public Map map;
    
    private Dictionary<EnumTeam, SpawnPosition> spawnPositions;

    private void Awake() {
        this.spawnPositions = new Dictionary<EnumTeam, SpawnPosition>();

        foreach(SpawnPosition sp in this.transform.GetComponentsInChildren<SpawnPosition>()) {
            this.spawnPositions.Add(sp.getTeam(), sp);
        }
    }

    public Vector3? getSpawnPosFromTeam(EnumTeam team) {
        if(this.spawnPositions.ContainsKey(team)) {
            return this.spawnPositions[team].transform.position;;
        } else {
            return null;
        }
    }

    public int getPlayerCount() {
        return this.spawnPositions.Count;
    }
}
