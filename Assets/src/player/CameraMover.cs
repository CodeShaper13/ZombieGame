using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//public class CameraMover : MonoBehaviour {

//    public static CameraMover localPlayer;

//    private Main main;

//    [SerializeField]
//    private EnumTeam objectTeam;
//    private Team team;

//    // Object references;
//    public Text resourceText;
//    public Text troopCountText;
//    public Camera mainCamera;

//    public SelectedParty selectedParty;
//    public SelectedBuilding selectedBuilding;
//    public SelectedDisplayerBase getSelected() {
//        if (this.selectedBuilding.isSelected()) {
//            return this.selectedBuilding;
//        }
//        else {
//            return this.selectedParty;
//        }
//    }

//    public ActionButtonManager actionButtons;

//    public SelectionBox selectionBox;

//    /// <summary>
//    /// Returns the instance of the camera mover that this game is using.
//    /// </summary>
//    public static CameraMover instance() {
//        return CameraMover.singleton;
//    }

//    private void Awake() {
//        CameraMover.singleton = this;

//        this.team = Team.getTeamFromEnum(this.objectTeam);
//        this.selectedParty.setCameraMover(this);
//    }

//    private void Update() {
//        // Deselect the selected building if it got destroyed.
//        if (this.selectedBuilding.isSelected() && !Util.isAlive(this.selectedBuilding.getBuilding())) {
//            this.selectedBuilding.clearSelected();
//            this.actionButtons.updateSideButtons();
//        }

//        foreach (UnitBase unit in this.selectedParty.getAllUnits()) {
//            //TODO
//        }
//    }

//    private void LateUpdate() {
//        // Debug keys.
//        if (Input.GetKeyDown(KeyCode.F3)) {
//            Main.DEBUG = !Main.DEBUG;
//            Debug.Log("Toggling Debug Mode.  It is now set to " + Main.DEBUG);
//        }

//        if (this.main.isPaused()) {
//            // Paused.
//            if (Input.GetKeyDown(KeyCode.Escape)) {
//                this.main.resumeGame();
//            }
//        }
//        else {
//            // Not paused.
//            if (Input.GetKeyDown(KeyCode.Escape)) {
//                // Escape was pressed, canceling things or if nothing can be canceled, pause the game.
//                if (this.actionButtons.delayedButtonRef != null) {
//                    this.actionButtons.cancelDelayedAction();
//                }
//                //else if (this.buildOutline.isEnabled()) {
//                //    this.buildOutline.disableOutline();
//                //}
//                else {
//                    this.actionButtons.closePopupButtons();
//                    this.main.pauseGame();
//                }
//            }
//            else {
//                this.handleCameraMovement();
//                if (!EventSystem.current.IsPointerOverGameObject()) {
//                    // Mouse over the playing field, not UI object.

//                    //if (this.buildOutline.isEnabled()) {
//                    //    this.buildOutline.handleClick();
//                    //}
//                    //else {
//                        this.handlePlayerInput();
//                    //}
//                }
//            }
//        }
//        this.updateHudCounts();
//    }

//    private void handlePlayerInput() {
//        bool leftBtnUp = Input.GetMouseButtonUp(0);
//        bool rightBtnUp = Input.GetMouseButtonUp(1);

//        this.selectionBox.updateRect();

//        if (leftBtnUp || rightBtnUp) {
//            RaycastHit hit;
//            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity)) {
//                SidedEntity entity = hit.transform.gameObject.GetComponent<SidedEntity>();
//                if (entity == null) {
//                    // Didn't click anything, move the party to the clicked point.
//                    if (leftBtnUp && hit.transform.CompareTag("Ground")) {
//                        this.actionButtons.closePopupButtons();
//                        this.selectedParty.moveAllTo(hit.point);
//                    }
//                    if (rightBtnUp) {
//                        this.actionButtons.closePopupButtons();
//                        this.getSelected().clearSelected();
//                        // Deselect all selected units.
//                    }
//                }
//                else {
//                    // Clicked an Entity
//                    if (this.actionButtons.delayedButtonRef != null) {
//                        ActionButtonRequireClick delayedButton = this.actionButtons.delayedButtonRef;
//                        // Check if this is a valid option to preform the action on.
//                        if (delayedButton.isValidForAction(this.team, entity)) {
//                            if (this.selectedBuilding.getBuilding() != null) {
//                                delayedButton.callFunction(this.selectedBuilding.getBuilding(), entity);
//                            }
//                            else {
//                                delayedButton.callFunction(this.selectedParty.getAllUnits(), entity);
//                            }
//                        }
//                    }
//                    else {
//                        if (entity.getTeam() == this.team) {
//                            // Clicked something on our team.
//                            if (leftBtnUp) {
//                                this.onLeftBtnClick(entity);
//                            }
//                            if (rightBtnUp) {
//                                this.onRightBtnClick(entity);
//                            }
//                        }
//                    }
//                }
//                // A click happened, so if something valid was clicked the delayed action was called.
//                // Either way, we should cancel the action becuase it was resolved or the wrong thing was clicked.
//                this.actionButtons.cancelDelayedAction();
//            }
//        }
//    }

//    private void onLeftBtnClick(SidedEntity entity) {
//        this.getSelected().clearSelected();
//        if (entity is UnitBase) {
//            this.selectedParty.tryAdd((UnitBase)entity);
//        }
//        else if (entity is BuildingBase) {
//            this.selectedBuilding.setSelected((BuildingBase)entity);
//        }
//        this.actionButtons.updateSideButtons();
//    }

//    private void onRightBtnClick(SidedEntity entity) {
//        if (entity is UnitBase) {
//            if (this.selectedParty.contains((UnitBase)entity)) {
//                this.selectedParty.remove((UnitBase)entity);
//            }
//            else {
//                this.selectedBuilding.clearSelected();
//                this.selectedParty.tryAdd((UnitBase)entity);
//            }
//        }
//        else if (entity is BuildingBase) {
//            if (entity == this.selectedBuilding) {
//                this.selectedBuilding.clearSelected();
//            }
//            else {
//                this.selectedParty.clearSelected();
//                this.selectedBuilding.setSelected((BuildingBase)entity);
//            }
//        }
//        this.actionButtons.updateSideButtons();
//    }

//    /// <summary>
//    /// Returns the team that controls this camera.
//    /// </summary>
//    public Team getTeam() {
//        return this.team;
//    }

//    /// <summary>
//    /// Centers the camera on the passed position.
//    /// </summary>
//    public void centerOn(Vector3 pos) {
//        this.setPostion(new Vector3(pos.x, this.getPosition().y, pos.z));
//    }

//    public void setPostion(Vector3 position) {
//        this.transform.parent.position = position;
//    }

//    public Vector3 getPosition() {
//        return this.transform.parent.position;
//    }

//    /// <summary>
//    /// Updates the troop and player count text on the corner of the screen.
//    /// </summary>
//    public void updateHudCounts() {
//        this.troopCountText.text = "Troops: " + 0 + "/" + 0;
//        this.resourceText.text = "Resources: " + 0 + "/" + 0;
//    }

//    /// <summary>
//    /// Moves and pans the camera based on the input form the user.
//    /// </summary>
//    private void handleCameraMovement() {
//        const int panSensitivity = 25;
//        const int zoomSensitivity = 300;

//        // Move position.
//        float forwardSpeed = (Input.GetAxis("Horizontal") * panSensitivity * Time.deltaTime);
//        float sideSpeed = (Input.GetAxis("Vertical") * panSensitivity * Time.deltaTime);
//        this.transform.parent.Translate(forwardSpeed, 0, sideSpeed);

//        // Zoom in and out.
//        float zoom = Input.GetAxis("Mouse ScrollWheel");
//        if (zoom != 0f) {
//            float f = zoom * Time.deltaTime * zoomSensitivity;
//            this.transform.Translate(0, 0, f);

//            float LOWEST_ZOOM = -12f;
//            float HIGHEST_ZOOM = -40f;

//            float localZ = this.transform.localPosition.z;
//            if (localZ > LOWEST_ZOOM) {
//                this.transform.Translate(0, 0, LOWEST_ZOOM - localZ);
//            }
//            else if (localZ < HIGHEST_ZOOM) {
//                this.transform.Translate(0, 0, HIGHEST_ZOOM - localZ);
//            }
//        }
//    }
//}