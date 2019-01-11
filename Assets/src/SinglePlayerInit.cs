using UnityEngine;

public class SinglePlayerInit : MonoBehaviour {

    private void Awake() {
        //GameObject.Instantiate(References.list.mapPrefab);

        Player player = GameObject.FindObjectOfType<Player>();
        player.team = Team.ORANGE;
        player.initUIs();
    }
}
