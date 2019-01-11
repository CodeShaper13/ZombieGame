using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorMountain : MapGenerator {

    public MapGeneratorMountain(Map map, int seed) : base(map, seed) { }

    public override void generateMap(MapData mapData) {
        List <Vector3> points = new List<Vector3>();

        // Place flags.
        foreach(Team team in Team.ALL_TEAMS) {
            Vector3? v = mapData.getSpawnPosFromTeam(team.getEnum());
            if(v != null) {
                Vector3 vec = (Vector3)v;
                points.Add(vec);
                vec.setY(this.getElevation(vec.x, vec.z));
                SpawnInstructions<SidedEntity> instructions = this.map.spawnEntity<SidedEntity>(Registry.buildingFlag, vec);
                instructions.getObj().setTeam(team);
            }
        }
        // Place trees and rocks.
        const float genZoneSize = 2.5f;
        const int mapRadius = 60;

        float x, z;
        int i;
        for(x = -mapRadius; x < mapRadius; x += genZoneSize) {
            for(z = -mapRadius; z < mapRadius; z += genZoneSize) {
                // Make sure the position is not to close to a starting point.
                bool flag = false;
                foreach(Vector3 point in points) {
                    if(Vector3.Distance(point, new Vector3(x, 0, z)) < 5f) {
                        flag = true;
                    }
                }
                if(flag) {
                    continue;
                }

                i = this.rndInt(0, 10000);
                if(i < 50) {
                    this.spawnRock(x, z);
                }
                else if(i < 400) {
                    this.spawnTree(x, z);
                }
            }
        }
    }
}
