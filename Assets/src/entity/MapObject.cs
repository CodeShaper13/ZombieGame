using UnityEngine;
using UnityEngine.Networking;

public abstract class MapObject : NetworkBehaviour {

    protected Map map;

    private void Awake() {
        this.map = Map.instance;

        this.onAwake();
    }

    private void Start() {
        this.onStart();
    }

    public override void OnStartServer() {
        base.OnStartServer();

        this.onServerInit();
    }

    /// <summary>
    /// Called when this MapObject is first spawned on both the server and client side.
    /// </summary>
    public virtual void onAwake() { }

    public virtual void onServerInit() { }

    public virtual void onStart() { }

    public virtual void onUpdate() { }

    public abstract Vector3 getFootPos();
}
