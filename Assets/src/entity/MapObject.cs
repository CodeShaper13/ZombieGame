using fNbt;
using System;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public abstract class MapObject : NetworkBehaviour, IDrawDebug, INbtSerializable {

    [HideInInspector]
    public Map map;

    /// <summary> Objects marked as immutable are destroyed via a button or edited (rotated, etc.) </summary>
    [SerializeField] // Used so it can be set in the inspecter.
    [SyncVar]
    private bool immutable;
    public Guid guid;

    public NbtCompound nbtTag;

    private void Awake() {
        this.map = Map.instance;
        //print("MapObject.Awake() was called and Map is (" + this.map + ")");

        this.onAwake();
    }

    private void Start() {
        this.onStart();
    }

    public override void OnStartClient() {
        base.OnStartClient();

        if(this.isClient) {
            //print("isClient = true");
        }
        if(!this.isServer) {
            this.map.addMapObject(this);
            //print("isServer = false");
        }

        this.onUiInit();
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

    private void OnDestroy() {
        if(!this.isServer) {
            this.map.mapObjects.Remove(this);
        }
    }

    /// <summary>
    /// Called when this MapObject is first spawned on both the server and client side.
    /// </summary>
    public virtual void onAwake() { }

    public virtual void onStart() { }

    /// <summary>
    /// A version of onAwake() that is only called on the server side after onAwake().
    /// </summary>
    public virtual void onServerAwake() {
        //print("serverAwake");
        this.guid = Guid.NewGuid();
    }

    /// <summary>
    /// Called after onStart() and should be used to initialize any world space UIs
    /// that this MapObject has.
    /// </summary>
    public virtual void onUiInit() {
        //print("onUiInit");
    }

    /// <summary>
    /// Called every frame to update this MapObject ont he server side.  Do not
    /// use Time.delta time, but instead the deltaTime parameter.
    /// </summary>
    [ServerSideOnly]
    public virtual void onUpdate(float deltaTime) { }

    /// <summary>
    /// Called every frame after onUpdate() to draw debug information on the screen.
    /// </summary>
    public virtual void drawDebug() { }

    /// <summary>
    /// Helper method to get the position of this object.
    /// </summary>
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

    /// <summary>
    /// Returns the display name of this Object.
    /// </summary>
    public abstract string getDisplayName();

    /// <summary>
    /// Reads the object from NBT and sets it's state.
    /// </summary>
    public virtual void readFromNbt(NbtCompound tag) {
        // Don't read id from NBT.

        this.transform.position = tag.getVector3("position");
        this.transform.eulerAngles = tag.getVector3("eulerRotation");
        this.transform.localScale = tag.getVector3("localScale", Vector3.one);
        this.immutable = tag.getBool("isImmutable");
        //this.guid = new Guid(tag.getString("guid"));
    }

    /// <summary>
    /// Writes the object to NBT.
    /// </summary>
    public virtual void writeToNbt(NbtCompound tag) {
        tag.setTag("id", Registry.getIdFromObject(this));

        tag.setTag("position", this.transform.position);
        tag.setTag("eulerRotation", this.transform.eulerAngles);
        tag.setTag("localScale", this.transform.localScale);
        tag.setTag("isImmutable", this.immutable);
        tag.setTag("guid", this.guid.ToString());
    }

    public static bool operator ==(MapObject lhs, MapObject rhs) {
        return MapObject.Equals(lhs, rhs);
    }

    public static bool operator !=(MapObject lhs, MapObject rhs) {
        return !MapObject.Equals(lhs, rhs);
    }

    public override bool Equals(object obj) {
        if(obj == null || this.GetType() != obj.GetType()) {
            return false;
        }
        Guid guid = ((MapObject)obj).getGuid();
        return this.getGuid().Equals(guid);
    }

    public override int GetHashCode() {
        return this.guid.GetHashCode();
    }
}
