using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ZombieGame/UnitStats", order = 1)]
public class UnitData : ScriptableObject {

    public string unitName;
    public int maxHealth;
    public int attack;
    public int defense;
}
