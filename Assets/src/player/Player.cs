using fNbt;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    /// <summary> Reference to this sides local player. </summary>
    public static Player localPlayer;

    private Map map;

    // UI references:
    [SerializeField]
    private SetupPhaseUI setupPhaseUI;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Text announcementText;
    [SerializeField]
    private Text resourceText;
    [SerializeField]
    private Text troopCountText;
    [SerializeField]
    /// <summary> A reference to the main canvas holding all of the UI elements. </summary>
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas canvasNoRaycast;

    // Selected object(s).
    public SelectedParty selectedParty;
    public SelectedBuilding selectedBuilding;
    public SelectedDisplayerBase getSelected() {
        if(this.selectedBuilding.isSelected()) {
            return this.selectedBuilding;
        }
        else {
            return this.selectedParty;
        }
    }

    // References to scripts on UI game objects.
    public ActionButtonManager actionButtons;
    public BuildOutline buildOutline;
    public SelectionBox selectionBox;

    public UnitDestinationEffect unitDestinationEffect;
    public AttackTargetEffect attackTargetEffect;

    private EnumGameState gameState;

    public Team team;
    public Camera playerCamera;

    private NetHandlerClient handler;

    [SyncVar]
    public int currentTeamResources;

    /// <summary> Time in seconds until the announcement text should vanish. </summary>
    private float announcementTimer;
    private Options options;

    private void Start() {
        this.map = GameObject.FindObjectOfType<Map>();
    }

    public override void OnStartClient() {
        this.handler = new NetHandlerClient(this);
    }

    public override void OnStartLocalPlayer() {
        this.initUIs();

        GameObject.FindObjectOfType<NetworkManagerHUD>().showGUI = false;
    }

    /// <summary>
    /// Reveals and initializes the Player's Camera and UI elements.
    /// </summary>
    public void initUIs() {
        Player.localPlayer = this;

        this.options = new Options();

        // Enable the Camera and HUD.
        this.playerCamera.gameObject.SetActive(true);
        this.mainCanvas.gameObject.SetActive(true);
        this.canvasNoRaycast.gameObject.SetActive(true);

        this.actionButtons.init();
        this.selectedParty.init(this);
        this.selectedBuilding.init(this);

        this.selectionBox.init(this);

        this.buildOutline.gameObject.SetActive(true);
        this.buildOutline.init(this);

        this.unitDestinationEffect = GameObject.Instantiate(References.list.prefabUnitDestinationEffect).GetComponent<UnitDestinationEffect>();
        this.attackTargetEffect = GameObject.Instantiate(References.list.prefabAttackTargetEffect).GetComponent<AttackTargetEffect>();
    }

    /*
    private void fetchCamera() {
        this.playerCamera = Camera.main;
        this.playerCamera.transform.parent = this.transform;
        this.playerCamera.transform.position = new Vector3(-20, 20, 0);
        this.playerCamera.transform.eulerAngles = new Vector3(45, 90, 0);
    }

    private void releaseCamera() {
        this.playerCamera.transform.parent = null;
    }
    */

    private void OnDestroy() {
        if(this.isLocalPlayer) {
            // If statements are for in the Editor, sometimes these objects are Destroyed before the Player.
            if(this.unitDestinationEffect != null) {
                GameObject.Destroy(this.unitDestinationEffect.gameObject);
            }
            if(this.attackTargetEffect != null) {
                GameObject.Destroy(this.attackTargetEffect.gameObject);
            }

            NetworkManagerHUD nmh = GameObject.FindObjectOfType<NetworkManagerHUD>();
            if(nmh != null) {
                nmh.showGUI = true;
            }
        }
    }

    private void LateUpdate() {
        if(this.isClient && Main.DEBUG) {
            foreach(MapObject obj in this.map.findMapObjects(null)) {
                obj.drawDebug();
            }
        }
    }

    private void Update() {
        // Only handle input if this is a local player.
        if(this.isLocalPlayer) {
            if(GuiManager.currentGui != null) {
                return;
            }

            // Debug keys.
            if(Input.GetKeyDown(KeyCode.F3)) {
                Main.DEBUG = !Main.DEBUG;
                Logger.log("Toggling Debug Mode.  It is now set to " + Main.DEBUG);
            }

            if(Main.instance().isPaused()) {
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    Main.instance().resumeGame();
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
                this.timerText.text = this.map.timer != -1 ? "Setup! " + Mathf.Ceil(this.map.timer).ToString() : string.Empty;

                this.selectedBuilding.onUpdate();
                this.selectedParty.onUpdate();

                this.updateHudCounts();

                // Only show this canvas if debug mode is on.
                this.setupPhaseUI.gameObject.SetActive(Main.DEBUG);

                if(Input.GetKeyDown(KeyCode.Escape)) {
                    // Escape was pressed, canceling things or if nothing can be canceled, pause the game.
                    if(this.actionButtons.getDelayedActionButton() != null) {
                        this.actionButtons.cancelDelayedAction();
                    }
                    else if(this.buildOutline.isEnabled()) {
                        this.buildOutline.disableOutline();
                    }
                    else {
                        this.actionButtons.closePopupButtons();
                        Main.instance().pauseGame();
                    }
                }
                else {
                    this.handleCameraMovement();
                    if(!EventSystem.current.IsPointerOverGameObject()) {
                        // Mouse over the playing field, not UI object.
                        if(this.buildOutline.isEnabled()) {
                            this.buildOutline.handleClick();
                        }
                        else {
                            this.handlePlayerInput();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Moves and pans the Camera based on the input from the user.
    /// </summary>
    private void handleCameraMovement() {
        // Move position.
        float sensitivity = this.options.sensitivity.get();
        float forwardSpeed = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;
        float sideSpeed = (Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime) * -1;
        this.transform.Translate(forwardSpeed, 0, sideSpeed);

        // Zoom in and out.
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if(zoom != 0) {
            float f = zoom * Time.deltaTime * this.options.zoomSensitivity.get();
            this.playerCamera.transform.Translate(0, 0, f);

            float LOWEST_ZOOM = -12f;
            float HIGHEST_ZOOM = -40f;

            float localZ = this.playerCamera.transform.localPosition.z;
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
            if(Physics.Raycast(this.playerCamera.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity)) {
                SidedEntity clickedEntity = hit.transform.gameObject.GetComponent<SidedEntity>();
                if(clickedEntity == null) {
                    TeamGround sidedGround = hit.transform.GetComponent<TeamGround>();
                    if(sidedGround != null && sidedGround.canInteractWith(this)) {
                        // Didn't click anything.
                        if(leftBtnUp) {
                            if(Input.GetKey(KeyCode.LeftShift) && Main.DEBUG) {
                                // Spawn something becuase shift was pressed.
                                PlaceableObject po = this.setupPhaseUI.getSelectedObject();
                                if(po != null && (po.getCount() > 0 || po.getCount() == -1)) {
                                    Team t = Input.GetKey(KeyCode.LeftControl) ? Team.BLUE : this.team;
                                    this.sendMessageToServer(new MessageSpawnEntity(po.registeredObject, hit.point, Vector3.zero, t));
                                    po.setCount(po.getCount() - 1);
                                }
                            }
                            else {
                                this.actionButtons.closePopupButtons();
                                bool unitMoved = this.selectedParty.moveAllTo(hit.point);
                                if(unitMoved) {
                                    this.unitDestinationEffect.setPosition(hit.point);
                                }
                            }
                        }
                        if(rightBtnUp) {
                            // Deselect all selected Units.
                            this.getSelected().clearSelected();
                        }
                    }
                }
                else {
                    // Clicked an Entity.
                    if(this.actionButtons.getDelayedActionButton() != null) {
                        ActionButtonRequireClick delayedButton = this.actionButtons.getDelayedActionButton();
                        // Check if this is a valid option to preform the action on.
                        if(delayedButton.isValidForAction(this.team, clickedEntity)) {
                            if(this.selectedBuilding.getBuilding() != null) {
                                delayedButton.callFunction(this.selectedBuilding.getBuilding(), clickedEntity);
                            }
                            else {
                                delayedButton.callFunction(this.selectedParty.getAllUnits(), clickedEntity);
                            }
                        }
                    }
                    else {
                        if(clickedEntity.getTeam() == this.team) {
                            // Clicked a SidedEntity that is on our team.
                            if(leftBtnUp) {
                                this.onLeftBtnClick(clickedEntity);
                            }
                            if(rightBtnUp) {
                                this.onRightBtnClick(clickedEntity);
                            }
                        }
                        else {
                            // Clicked a SidedEntity not on our team.

                            // Set all of the selected units that have the AttackNearby action to attack the clicked unit.
                            int mask = ActionButton.unitAttackNearby.getMask();
                            bool flag = false;
                            foreach(UnitBase unit in this.selectedParty.getAllUnits()) {
                                if((unit.getButtonMask() & mask) != 0) {
                                    this.sendMessageToServer(new MessageAttackSpecific(unit, clickedEntity));
                                    flag = true;
                                }
                            }

                            if(flag) {
                                this.attackTargetEffect.setTarget(clickedEntity);
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
            if(entity == this.selectedBuilding) {
                this.selectedBuilding.clearSelected();
            }
            else {
                this.selectedParty.clearSelected();
                this.selectedBuilding.setSelected((BuildingBase)entity);
            }
        }
    }

    public void setGameState(EnumGameState newState) {
        this.gameState = newState;
        //        this.setupPhaseUI.gameObject.SetActive(this.gameState == EnumGameState.PREPARE);
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

        Color c = this.team.getColor();
        this.resourceText.color = c;
        this.troopCountText.color = c;
    }

    /// <summary>
    /// Returns the team that this player controls.
    /// </summary>
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
        int maxResources = this.team.getMaxResourceCount(this.map);
        foreach(SidedEntity o in this.map.findMapObjects(this.team.predicateThisTeam)) {
            if(o is UnitBase) {
                currentTroopCount++;
            }
        }

        int max = this.team.getMaxTroopCount(this.map);
        this.troopCountText.text = "Troops: " + currentTroopCount + "/" + max;
        this.troopCountText.fontStyle = (currentTroopCount == max ? FontStyle.Bold : FontStyle.Normal);

        int res = this.currentTeamResources;
        this.resourceText.text = "Resources: " + res + "/" + maxResources;
        this.resourceText.fontStyle = (res == maxResources ? FontStyle.Bold : FontStyle.Normal);
    }

    /// <summary>
    /// Sends a message to the server.
    /// </summary>
    public void sendMessageToServer(AbstractMessageServer message) {
        base.connectionToServer.Send(message.getID(), message);
    }

    public void callActionButton(ActionButton button, List<SidedEntity> targets) {
        if(button.executeOnClientSide()) {
            button.callFunction(targets);
        }
        else {
            MessageRunAction msg;
            if(button is ActionButtonChild) {
                ActionButtonChild abc = (ActionButtonChild)button;
                msg = new MessageRunAction(abc.parentActionButton.getID(), abc.index, targets);
            }
            else {
                msg = new MessageRunAction(button.getID(), targets);
            }

            this.sendMessageToServer(msg);
        }
    }

    /// <summary>
    /// Enables the build outline.
    /// </summary>
    public void enableBuildOutline(RegisteredObject registeredBuilding, UnitBuilder builder) {
        this.buildOutline.enableOutline(registeredBuilding, builder);
    }

    public void writeToNbt(NbtCompound tag) {
        tag.setTag("playerPosition", this.transform.position);
        tag.setTag("resources", this.currentTeamResources);
    }

    public void readFromNbt(NbtCompound tag) {
        this.transform.position = tag.getVector3("playerPosition");
        this.currentTeamResources = tag.getInt("resources");
    }
}
