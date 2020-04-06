using fNbt;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents a Map object that is both alive (LivingObject) and has a team it belongs to.
/// </summary>
public abstract class SidedEntity : LivingObject {

    [SyncVar]
    private int teamID;

    [SerializeField]
    [Header("Set via inspecttor to pick team.")]
    private EnumTeam team;

    public override void onAwake() {
        base.onAwake();

        if(this.team != EnumTeam.NONE) {
            this.teamID = Team.getTeamFromEnum(this.team).getId();
        }
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

    public virtual Team getTeam() {
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
    /// Called when this Entity is selected by the Player.
    /// </summary>
    [ClientSideOnly]
    public virtual void onSelect() { } //TODO not yet called.

    /// <summary>
    /// Called when this Entity is deselected by the Player.
    /// </summary>
    [ClientSideOnly]
    public virtual void onDeselect() { } //TODO not yet called.
}
