using System;
using UnityEngine;

[Serializable]
public class TeamBaseData {
    public EnumTeam team;
    [Header("Spawned objects will face the same direction of this transform.")]
    public Transform orginPoint;
}