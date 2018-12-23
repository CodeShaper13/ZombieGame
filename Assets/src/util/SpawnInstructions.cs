using UnityEngine.Networking;

public class SpawnInstructions<T> where T : MapObject {

    private T obj;

    public SpawnInstructions(T obj) {
        this.obj = obj;    
    }

    public T getObj() {
        return this.obj;
    }

    public void spawn() {
        NetworkServer.Spawn(this.obj.gameObject);
    }
}
