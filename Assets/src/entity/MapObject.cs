using System;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public abstract class MapObject : NetworkBehaviour, IDrawDebug {

    [HideInInspector]
    public Map map;

    /// <summary> Objects marked as immutable are destroyed via a button or edited (rotated, etc.) </summary>
    [SerializeField] // Used so it can be set in the inspecter.
    [SyncVar]
    private bool immutable;
    private Guid guid;

    private void Awake() {
        this.map = Map.instance;

        this.onAwake();
    }

    private void Start() {
        this.onStart();
    }

    public override void OnStartServer() {
        base.OnStartServer();

        this.guid = Guid.NewGuid();

        this.onServerInit();
    }

    public override void OnStartClient() {
        base.OnStartClient();
        print("onStartClient");
        if(this.isClient) {
            print("isClient = true");
        }
        if(!this.isServer) {
            Map.instance.mapObjects.Add(this);
            print("isServer = true");
        }
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll) {
        if(forceAll) {
            writer.Write(this.guid.ToString());
            return true;
        }
        return false;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState) {
        if(initialState) {
            this.guid = new Guid(reader.ReadString());
        }
    }

    /// <summary>
    /// Called when this MapObject is first spawned on both the server and client side.
    /// </summary>
    public virtual void onAwake() { }

    public virtual void onServerInit() { }

    public virtual void onStart() { }

    public virtual void onUpdate(float deltaTime) { }

    public virtual void drawDebug() { }

    public Vector3 getPos() {
        return this.transform.position;
    }

    public Guid getGuid() {
        return this.guid;
    }

    /// <summary>
    /// Returns true if the object is immutable.
    /// </summary>
    public bool isImmutable() {
        return this.immutable;
    }
}
