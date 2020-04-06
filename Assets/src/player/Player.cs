using fNbt;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    /// <summary> Reference to this sides local player. </summary>
    public static Player localPlayer;

    private MapBase map;

    // Debug Fields
    public Team teamToDebugPlace;
    private bool showDataEditor;

    [SerializeField]
    private UnitDestinationEffect unitDestinationEffect = null;
    [SerializeField]
    private AttackTargetEffect attackTargetEffect = null;
    [SerializeField]
    private Text resourceText = null;
    [SerializeField]
    private Text troopCountText = null;
    public AnnouncementText announcementText = null;

    // Selected object(s).
    public SelectedParty selectedParty;
    public SelectedBuilding selectedBuilding;
    public SelectedDisplayerBase getSelected() {
        if(this.selectedBuilding.isSelected()) {
            return this.selectedBuilding;
        } else {
            return this.selectedParty;
        }
    }

    // References to scripts on UI game objects.
    public ActionButtonManager actionButtons;
    public BuildOutline buildOutline;
    public SelectionBox selectionBox;
    private Camera playerCam;

    public Team team;

    private NetHandlerClient handler;

    [SyncVar] // Resources are not saved here, only sent here from the server
    public int currentTeamResources;

    private Options options;

    private void Start() {
        this.map = GameObject.FindObjectOfType<MapBase>();
    }

    public override void OnStartClient() {
        this.handler = new NetHandlerClient(this);
    }

    public override void OnStartLocalPlayer() {
        Player.localPlayer = this;

        this.initUIs();
    }

    private void OnGUI() {
        if(this.showDataEditor) {
            const int height = 20;
            KeyedSettings ks = Constants.ks;
            int y = 0;
            foreach(KeyValuePair<string, KeyedSettings.SettingEntry> kpv in ks.dict) {
                KeyedSettings.SettingEntry setting = kpv.Value;

                GUI.Label(new Rect(10, y, 300, height), kpv.Key);
                string s = GUI.TextField(new Rect(310, y, 250, height), setting.value.ToString());

                y += height;
            }
        }
    }

    /// <summary>
    /// Reveals and initializes the Player's Camera and UI elements.
    /// </summary>
    public void initUIs() {
        this.options = new Options();

        // Enable the Camera and the HUD (They are children objects).
        this.transform.GetChild(0).gameObject.SetActive(true);

        this.playerCam = this.GetComponentInChildren<Camera>();

        this.actionButtons.init(this);
        this.selectedParty.init(this);
        this.selectedBuilding.init(this);
    }

    private void LateUpdate() {
        if(this.isClient && Main.DEBUG) {
            this.map.drawDebug();

            foreach(MapObject obj in this.map.findMapObjects(null)) {
                obj.drawDebug();
            }
        }
    }

    private void Update() {
        // Only handle input if this is a local player.
        if(this.isLocalPlayer) {
            if(GuiManager.currentGui.Count != 0) {
                return;
            }

            // Debug keys.
            if(Input.GetKeyDown(KeyCode.F2)) {
                ScreenshotHelper.captureScreenshot();
            }
            if(Input.GetKeyDown(KeyCode.F3)) {
                Main.DEBUG = !Main.DEBUG;
                Logger.log("Toggling Debug Mode.  It is now set to " + Main.DEBUG);
            }
            if(Input.GetKey(KeyCode.F4)) {
                this.map.setResources(this.team, this.map.getResources(this.team) + 100);
            }
            if(Input.GetKeyDown(KeyCode.F5)) {
                this.showDataEditor = !this.showDataEditor;
            }

            if(Pause.isPaused()) {
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    Main.instance().resumeGame();
                }
            } else {
                this.updateHudCounts();

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
                } else {
                    this.handleCameraMovement();
                    if(!EventSystem.current.IsPointerOverGameObject()) {
                        // Mouse over the playing field, not UI object.
                        if(this.buildOutline.isEnabled()) {
                            this.buildOutline.handleClick();
                        } else {
                            this.handlePlayerInput();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Moves the Camera based on the input from the user.
    /// </summary>
    private void handleCameraMovement() {
        float sensitivity = this.options.sensitivity.get();
        float upDown = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;
        float sideToSide = Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;
        this.playerCam.transform.Translate(sideToSide, upDown, 0);
    }

    private void handlePlayerInput() {
        // DEBUG
        if(Input.GetKeyDown(KeyCode.Y)) {
            Vector3 mouseWorldPos = this.playerCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int v = TileMaps.singleton.worldToCell(mouseWorldPos + Vector3.one * 0.5f);
            this.sendMessageToServer(new MessageSpawnEntity(Registry.unitGunner, new Vector3(v.x, v.y), Vector3.zero, this.teamToDebugPlace != null ? this.teamToDebugPlace : this.team));
        }
        // End DEBUG

        bool leftBtnUp = Input.GetMouseButtonUp(0);
        bool rightBtnUp = Input.GetMouseButtonUp(1);

        this.selectionBox.updateRect();

        if(leftBtnUp || rightBtnUp) {
            Vector3 mousePos = this.playerCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider == null) {
                // Clicked nothing.
                if(leftBtnUp) {
                    this.actionButtons.closePopupButtons();
                    bool unitMoved = this.selectedParty.moveAllTo(mousePos);
                    if(unitMoved) {
                        this.unitDestinationEffect.setPosition(mousePos);
                    }
                }
                if(rightBtnUp) {
                    // Deselect all selected Units.
                    this.getSelected().clearSelected();
                }
            } else {
                // Clicked something.
                LivingObject livingObject = hit.transform.GetComponent<LivingObject>();
                if(livingObject != null) {
                    // Clicked an Entity.
                    if(this.actionButtons.getDelayedActionButton() != null) {
                        ActionButtonRequireClick delayedButton = this.actionButtons.getDelayedActionButton();
                        // Check if this is a valid option to preform the action on.
                        if(delayedButton.isValidForAction(this.team, livingObject)) {
                            if(this.selectedBuilding.getBuilding() != null) {
                                delayedButton.callFunction(this.selectedBuilding.getBuilding(), livingObject);
                            }
                            else {
                                delayedButton.callFunction(this.selectedParty.getAllUnits(), livingObject);
                            }
                        }
                    } else {
                        if(livingObject is SidedEntity) {
                            SidedEntity clickedEntity = (SidedEntity)livingObject;
                            if(clickedEntity.getTeam() == this.team) {
                                // Clicked a SidedEntity that is on our team.
                                if(leftBtnUp) {
                                    this.onLeftBtnClick(clickedEntity);
                                }
                                if(rightBtnUp) {
                                    this.onRightBtnClick(clickedEntity);
                                }
                            } else {
                                // Clicked a SidedEntity not on our team.

                                // Set all of the selected units that have the AttackNearby action to attack the clicked unit, only if they are already not attacking something.
                                int mask = ActionButton.unitAttackNearby.getMask();
                                bool flag = false;
                                foreach(UnitBase unit in this.selectedParty.getAllUnits()) {
                                    if((unit.getButtonMask() & mask) != 0) { // Unit has the attack Action Button
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
        this.actionButtons.closePopupButtons();
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

    public Vector2Int getMousePos() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int coords = TileMaps.singleton.worldToCell(mouseWorldPos + Vector3.one * 0.5f);
        return new Vector2Int(coords.x, coords.y);
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
        this.playerCam.transform.position = new Vector3(pos.x, pos.y, this.playerCam.transform.position.z);
    }

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
        //this.troopCountText.fontStyle = (currentTroopCount == max ? FontStyle.Bold : FontStyle.Normal);

        int res = this.currentTeamResources;
        this.resourceText.text = "Resources: " + res + "/" + maxResources;
        //this.resourceText.fontStyle = (res == maxResources ? FontStyle.Bold : FontStyle.Normal);
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
        tag.setTag("playerPosition", this.playerCam.transform.position);
    }

    public void readFromNbt(NbtCompound tag) {
        this.playerCam.transform.position = tag.getVector3("playerPosition");
    }
}
