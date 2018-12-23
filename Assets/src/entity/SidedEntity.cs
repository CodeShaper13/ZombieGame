using cakeslice;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Outline))]
public abstract class SidedEntity : LivingObject {

    [SyncVar]
    private int teamID;
    private OutlineHelper outlineHelper;

    private void OnMouseDrag() {
        if(this.isLocalPlayer) {
            if(Player.localPlayer.getGameState() == EnumGameState.PREPARE && this.getTeam() == Player.localPlayer.getTeam()) {
                Camera main = Camera.main;
                float DistanceToScreen = main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 posMove = main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceToScreen));
                Vector3 v = new Vector3(posMove.x, this.transform.position.y, posMove.z);
                //this.transform.position = v;

                //Send a message to the server
                Player.localPlayer.sendMessageToServer(new MessageSetObjectPostion(this.gameObject, v));
            }
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();

        this.outlineHelper = new OutlineHelper(this.gameObject);
        this.setOutlineVisibility(false, EnumOutlineParam.ALL);

        this.colorObject();
    }

    /// <summary>
    /// Returns the bitmask of what buttons to display.
    /// </summary>
    public virtual int getButtonMask() {
        return ActionButton.destroy.getMask();
    }

    public Team getTeam() {
        return Team.ALL_TEAMS[this.teamID];
    }

    public void setTeam(Team team) {
        this.teamID = team.getTeamId();
    }

    /// <summary>
    /// Colors this object based on it's team.
    /// </summary>
    public virtual void colorObject() { }

    public SidedEntity getClosestEnemyObject() {
        SidedEntity closest = null;
        float d = float.PositiveInfinity;
        foreach (SidedEntity s in this.map.mapObjects) {
            if(s.getTeam() != this.getTeam()) {
                float f = Vector3.Distance(this.transform.position, s.transform.position);
                if (f < d) {
                    d = f;
                    closest = s;
                }
            }
        }
        return closest;
    }

    /// <summary>
    /// Sets if the outline is visible.  An optional number can be passed to set the outline color.
    /// </summary>
    public virtual void setOutlineVisibility(bool visible, EnumOutlineParam type) {
        this.outlineHelper.updateOutline(visible, type);
    }
}
