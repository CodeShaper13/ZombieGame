using cakeslice;
using fNbt;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Outline))]
public abstract class SidedEntity : LivingObject {

    [SyncVar]
    [SerializeField]
    private int teamID;
    private OutlineHelper outlineHelper;

    /*
    private void OnMouseDrag() {
        if(this.isLocalPlayer) {
            if(Player.localPlayer.getGameState() == EnumGameState.PREPARE && this.getTeam() == Player.localPlayer.getTeam()) {
                Camera main = Camera.main;
                float DistanceToScreen = main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 posMove = main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceToScreen));
                Vector3 v = new Vector3(posMove.x, this.transform.position.y, posMove.z);

                //Send a message to the server
                Player.localPlayer.sendMessageToServer(new MessageSetObjectPostion(this.gameObject, v));
            }
        }
    }
    */

    [ClientSideOnly]
    public override void onUiInit() {
        this.outlineHelper = new OutlineHelper(this.gameObject);
        this.setOutlineVisibility(false, EnumOutlineParam.ALL);

        base.onUiInit(); // Base need to be called the OutlineHelper is set up.

        this.colorObject();
    }

    /// <summary>
    /// Returns the bitmask of what buttons to display.
    /// </summary>
    public virtual int getButtonMask() {
        return ActionButton.entityDestroy.getMask();
    }

    [ServerSideOnly]
    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.teamID = tag.getInt("teamId"); // TODO Should the setter be called here?
    }

    [ServerSideOnly]
    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("teamId", this.teamID);
    }

    public Team getTeam() {
        return Team.ALL_TEAMS[this.teamID];
    }

    public void setTeam(Team team) {
        this.teamID = team.getId();
    }

    /// <summary>
    /// Colors this object based on it's Team.
    /// </summary>
    public virtual void colorObject() { }

    /// <summary>
    /// Sets if the outline is visible.  An optional number can be passed to set the outline color.
    /// </summary>
    public virtual void setOutlineVisibility(bool visible, EnumOutlineParam type) {
        this.outlineHelper.updateOutline(visible, type);
    }

    /// <summary>
    /// Called when this Entity is selected by the Player.
    /// </summary>
    [ClientSideOnly]
    public virtual void onSelect() {
    } //TODO not yet called.

    /// <summary>
    /// Called when this Entity is deselected by the Player.
    /// </summary>
    [ClientSideOnly]
    public virtual void onDeselect() {
    } //TODO not yet called.
}
