using UnityEngine;
using UnityEngine.Networking;

public class GameInitializer : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    private void Awake() {
        if(NetworkManager.singleton == null) {
            GameObject.Instantiate(this.prefab);
        }
    }

#if UNITY_EDITOR
    private void Start() {
        print("gmae init!");
        //GameObject.FindObjectOfType<NetworkManager>().StartHost();
    }
#endif
}
