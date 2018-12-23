using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    /// <summary> Reference to this sides local player. </summary>
    public static Player localPlayer;

    [HideInInspector]
    public Main main;

    public Canvas mainCanvas;

    [SerializeField]
    private SetupPhaseUI setupPhaseUI;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Text announcementText;
    private float announcementTimer;

    [SerializeField]
    public Text resourceText;
    [SerializeField]
    public Text troopCountText;

    public SelectedParty selectedParty;
    public SelectedBuilding selectedBuilding;
    public SelectedDisplayerBase getSelected() {
        if(this.selectedBuilding.isSelected()) {
            return this.selectedBuilding;
        } else {
            return this.selectedParty;
        }
    }

    public ActionButtonManager actionButtons;
    public SelectionBox selectionBox;

    private EnumGameState gameState;

    public Team team;
    public GameObject cameraObj;

    private NetHandlerClient handler;

    public override void OnStartClient() {
        this.handler = new NetHandlerClient(this);
    }

    public override void OnStartLocalPlayer() {
        Player.localPlayer = this;
        this.main = Main.instance();

        // Enable the Camera and HUD.
        this.cameraObj.SetActive(true);
        this.mainCanvas.gameObject.SetActive(true);

        this.actionButtons.init();
        this.selectedParty.init(this);
        this.selectedBuilding.init(this);
    }

    private void Update() {
        // Only handle input if this is a local player.
        if(this.isLocalPlayer) {
            // Debug keys.
            if(Input.GetKeyDown(KeyCode.F3)) {
                Main.DEBUG = !Main.DEBUG;
                Debug.Log("Toggling Debug Mode.  It is now set to " + Main.DEBUG);
            }

            if(this.main.isPaused()) {
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    this.main.resumeGame();
                }
            }
            else {
                // Handle announcement text.
                if(this.announcementTimer > 0) {
                    this.announcementTimer -= Time.deltaTime;
                    if(this.announcementTimer <= 0) {
                        this.announcementText.text = string.Empty;
                    }
                }

                // Update timer text.
                this.timerText.text = Map.instance.timer != -1 ? "Setup! " + Mathf.Ceil(Map.instance.timer).ToString() : string.Empty;

                this.selectedBuilding.onUpdate();
                this.selectedParty.onUpdate();

                this.updateHudCounts();

                // Not paused.
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    // Escape was pressed, canceling things or if nothing can be canceled, pause the game.
                    if(this.actionButtons.delayedButtonRef != null) {
                        this.actionButtons.cancelDelayedAction();
                    }
                    //else if(this.buildOutline.isEnabled()) {
                    //    this.buildOutline.disableOutline();
                    //}
                    else {
                        this.actionButtons.closePopupButtons();
                        this.main.pauseGame();
                    }
                }
                else {
                    this.handleCameraMovement();
                    if(!EventSystem.current.IsPointerOverGameObject()) {
                        // Mouse over the playing field, not UI object.

                        //if(this.buildOutline.isEnabled()) {
                        //    this.buildOutline.handleClick();
                        //}
                        //else {
                            this.handlePlayerInput();
                        //}
                    }
                }
            }
        }
    }

    /// <summary>
    /// Moves and pans the camera based on the input form the user.
    /// </summary>
    private void handleCameraMovement() {
        // Move position.
        const float sensitivity = 40f;
        float forwardSpeed = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;
        float sideSpeed = (Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime) * -1;
        this.transform.Translate(forwardSpeed, 0, sideSpeed);

        // Zoom in and out.
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if(zoom != 0) {
            const float zoomSensitivity = 300;
            float f = zoom * Time.deltaTime * zoomSensitivity;
            this.cameraObj.transform.Translate(0, 0, f);

            float LOWEST_ZOOM = -12f;
            float HIGHEST_ZOOM = -40f;

            float localZ = this.cameraObj.transform.localPosition.z;
            if(localZ > LOWEST_ZOOM) {
                //this.cameraObj.transform.Translate(0, 0, LOWEST_ZOOM - localZ);
            }
            else if(localZ < HIGHEST_ZOOM) {
                //this.cameraObj.transform.Translate(0, 0, HIGHEST_ZOOM - localZ);
            }
        }
    }

    private void handlePlayerInput() {
        bool leftBtnUp = Input.GetMouseButtonUp(0);
        bool rightBtnUp = Input.GetMouseButtonUp(1);

        this.selectionBox.updateRect();

        if(leftBtnUp || rightBtnUp) {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity)) {
                SidedEntity entity = hit.transform.gameObject.GetComponent<SidedEntity>();
                if(entity == null) {
                    // Didn't click anything, move the party to the clicked point.
                    TeamGround tg = hit.transform.GetComponent<TeamGround>();
                    if(tg != null) {
                        if(Input.GetKey(KeyCode.LeftShift)) {
                            PlaceableObject po = this.setupPhaseUI.getSelectedObject();
                            if(po != null && po.getCount() > 0) {
                                Team t = Input.GetKey(KeyCode.LeftControl) ? Team.BLUE : this.team;
                                this.sendMessageToServer(new MessageSpawnEntity(po.registeredObject, hit.point, Vector3.zero, t));
                                po.setCount(po.getCount() - 1);
                            }
                        } else {
                            //if(leftBtnUp && hit.transform.CompareTag(Tags.ground)) {
                            this.actionButtons.closePopupButtons();
                            this.selectedParty.moveAllTo(hit.point);
                        }

                        if(rightBtnUp) {
                            this.actionButtons.closePopupButtons();
                            this.getSelected().clearSelected();
                            // Deselect all selected Units.
                        }
                    }
                }
                else {
                    // Clicked an Entity.
                    if(this.actionButtons.delayedButtonRef != null) {
                        ActionButtonRequireClick delayedButton = this.actionButtons.delayedButtonRef;
                        // Check if this is a valid option to preform the action on.
                        if(delayedButton.isValidForAction(this.team, entity)) {
                            if(this.selectedBuilding.getBuilding() != null) {
                                delayedButton.callFunction(this.selectedBuilding.getBuilding(), entity);
                            }
                            else {
                                delayedButton.callFunction(this.selectedParty.getAllUnits(), entity);
                            }
                        }
                    }
                    else {
                        if(entity.getTeam() == this.team) {
                            // Clicked something on our team.
                            if(leftBtnUp) {
                                this.onLeftBtnClick(entity);
                            }
                            if(rightBtnUp) {
                                this.onRightBtnClick(entity);
                            }
                        }
                    }
                }
                // A click happened, so if something valid was clicked the delayed action was called.
                // Either way, we should cancel the action becuase it was resolved or the wrong thing was clicked.
                this.actionButtons.cancelDelayedAction();
            }
        }
    }

    private void onLeftBtnClick(SidedEntity entity) {
        this.getSelected().clearSelected();
        if(entity is UnitBase) {
            this.selectedParty.tryAdd((UnitBase)entity);
        }
        else if(entity is BuildingBase) {
            this.selectedBuilding.setSelected((BuildingBase)entity);
        }
        this.actionButtons.updateSideButtons();
    }

    private void onRightBtnClick(SidedEntity entity) {
        if(entity is UnitBase) {
            if(this.selectedParty.contains((UnitBase)entity)) {
                this.selectedParty.remove((UnitBase)entity);
            }
            else {
                this.selectedBuilding.clearSelected();
                this.selectedParty.tryAdd((UnitBase)entity);
            }
        }
        else if(entity is BuildingBase) {
            if(entity == this.selectedBuilding) { //TODO == no longer works becuase we don't have Guids???
                this.selectedBuilding.clearSelected();
            }
            else {
                this.selectedParty.clearSelected();
                this.selectedBuilding.setSelected((BuildingBase)entity);
            }
        }
        this.actionButtons.updateSideButtons();
    }

    public void setGameState(EnumGameState newState) {
        this.gameState = newState;
        this.setupPhaseUI.gameObject.SetActive(this.gameState == EnumGameState.PREPARE);
    }

    public EnumGameState getGameState() {
        return this.gameState;
    }

    /// <summary>
    /// Shows the passed text to the player as an announcement.
    /// </summary>
    public void showAnnouncement(string msg, float duration) {
        this.announcementText.text = msg;
        this.announcementTimer = duration;
    }

    [ClientRpc]
    public void RpcSetTeam(int newTeamId) {
        this.team = Team.getTeamFromId(newTeamId);
        Color color = this.team.getTeamColor();
        this.GetComponent<MeshRenderer>().material.color = color;
    }

    public Team getTeam() {
        return this.team;
    }

    /// <summary>
    /// Centers the camera on the passed position.
    /// </summary>
    public void centerCameraOn(Vector3 pos) {
        this.transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
    }

    /*
    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    private void CmdFire() {
        // Create the Bullet from the Bullet Prefab
        //GameObject bullet = GameObject.Instantiate(
        //    Registry.projectileBullet.getPrefab(),
        //    this.bulletSpawn.position,
        //    this.bulletSpawn.rotation);

        // Add velocity to the bullet
        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 60;

        // Spawn the bullet on the Clients
        //NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        //GameObject.Destroy(bullet, 2.0f);
    }
    */

    /// <summary>
    /// Updates the troop and player count text on the corner of the screen.
    /// </summary>
    public void updateHudCounts() {
        int currentTroopCount = 0;
        int maxResources = this.team.getMaxResourceCount();
        foreach(SidedEntity o in Map.instance.findMapObjects(this.team.predicateThisTeam)) {
            if(o is UnitBase) {
                currentTroopCount++;
            }
        }

        int max = this.team.getMaxTroopCount();
        this.troopCountText.text = "Troops: " + currentTroopCount + "/" + max;
        //this.troopCountText.fontStyle = (currentTroopCount == max ? FontStyle.Bold : FontStyle.Normal);

        int res = this.team.getResources();
        this.resourceText.text = "Resources: " + res + "/" + maxResources;
        //this.resourceText.fontStyle = (res == maxResources ? FontStyle.Bold : FontStyle.Normal);
    }

    /// <summary>
    /// Sends a message to the server.
    /// </summary>
    public void sendMessageToServer(AbstractMessage<NetHandlerServer> message) {
        base.connectionToServer.Send(message.getID(), message);
    }

    public void callActionButton(ActionButton button, params SidedEntity[] targets) {
        MessageRunAction msg;
        if(button is ActionButtonChild) {
            ActionButtonChild abc = (ActionButtonChild)button;
            msg = new MessageRunAction(abc.parentActionButton.getID(), abc.index, targets);
        } else {
            msg = new MessageRunAction(button.getID(), targets);
        }

        this.sendMessageToServer(msg);
    }
}
