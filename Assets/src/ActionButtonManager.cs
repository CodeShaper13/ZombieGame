using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour {

    /// <summary>
    /// The maximum number of buttons that can be shown at once.  If any more 
    /// are tried to be shown, there will be problems.
    /// </summary>
    private const int MAX_BUTTONS = 10;

    [SerializeField]
    private GameObject buttonPrefab;

    private Transform subButtonCanvas;

    private Player player;
    private ButtonWrapper[] buttonWrappers;
    private ButtonWrapper[] subButtonWrappers;
    /// <summary> A list of the current buttons that are shown on the side of the screen.  This does NOT contain the sub buttons. </summary>
    private List<ActionButton> currentlyShownButtons;
    /// <summary> Saves what button was clicked when dealing with sub buttons. </summary>
    private ActionButton selectedMainButton;
    /// <summary> Stores if the buttons should be forced to be disabled (not visible).  The BuildOutline enables this. </summary>
    private bool forceDisabled;

    private ActionButtonRequireClick delayedButtonRef;
    private List<SidedEntity> selectedOutlineObjects;

    public void init() {
        this.player = this.GetComponentInParent<Player>();

        this.subButtonCanvas = this.transform.GetChild(0);
        this.currentlyShownButtons = new List<ActionButton>();

        this.buttonWrappers = this.makeButtons(this.transform, false);
        this.subButtonWrappers = this.makeButtons(this.subButtonCanvas, true);
        
        this.selectedOutlineObjects = new List<SidedEntity>();
    }

    private void LateUpdate() {
        this.updateSideButtons();

        // Update if the buttons are interactable or not.
        if(!this.forceDisabled) {
            for(int i = 0; i < this.currentlyShownButtons.Count; i++) {
                ActionButton actionButton = this.currentlyShownButtons[i];
                this.checkButtonInteractability(actionButton, this.buttonWrappers[i]);
                this.buttonWrappers[i].setInteractable(this.selectedMainButton == null || this.selectedMainButton == actionButton);

                // Preform the check for child buttons.
                if(this.selectedMainButton == actionButton) {
                    ActionButton[] subButtons = ((ActionButtonParent)actionButton).getChildButtons();
                    for(int j = 0; j < subButtons.Length; j++) {
                        this.checkButtonInteractability(subButtons[j], this.subButtonWrappers[j]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Shows the correct buttons on the side of the screen based on the selected units or building.
    /// </summary>
    private void updateSideButtons() {
        int selectedMask = this.player.getSelected().getMask();
        this.currentlyShownButtons.Clear();

        // Iterate through all of the action buttons, adding the ones to a list that need to be displayed.
        for(int i = 0; i < ActionButton.BUTTON_LIST.Length; i++) {
            ActionButton button = ActionButton.BUTTON_LIST[i];
            if(button != null && ((selectedMask >> i) & 1) == 1) {
                this.currentlyShownButtons.Add(button);
            }
        }

        // Set the button text on the screen.
        for(int i = 0; i < MAX_BUTTONS; i++) {
            if(i < this.currentlyShownButtons.Count) {
                ActionButton actionButton = this.currentlyShownButtons[i];
                this.buttonWrappers[i].setText(actionButton.getText());
                this.buttonWrappers[i].setVisible(!this.forceDisabled);
            }
            else {
                this.buttonWrappers[i].setVisible(false);
            }
        }

        // Hide the sub button's if their parent button is no longer displayed.
        if(!this.currentlyShownButtons.Contains(this.selectedMainButton)) {
            this.closePopupButtons();
        }
    }

    /// <summary>
    /// Closes the popup buttons and makes it so you can interacted with the main buttons again.
    /// </summary>
    public void closePopupButtons() {
        for(int i = 0; i < MAX_BUTTONS; i++) {
            // Hide sub buttons.
            this.subButtonWrappers[i].setVisible(false);

            // Enable the main buttons.
            this.buttonWrappers[i].setInteractable(true);
        }

        this.selectedMainButton = null;
    }

    /// <summary>
    /// Forces all of the action buttons to be disabled.  Pass false to remove this effect.
    /// </summary>
    public void setForceDisabled(bool disabled) {
        this.forceDisabled = disabled;
        if(this.forceDisabled) {
            this.closePopupButtons();
        }
        foreach(ButtonWrapper bw in this.buttonWrappers) {
            bw.setInteractable(!this.forceDisabled);
        }
    }

    private bool isPoupShown() {
        return this.selectedMainButton != null;
    }

    /// <summary>
    /// Called every time a button is clicked.
    /// </summary>
    /// <param name="index"> The index of the clicked button. </param>
    /// <param name="isSubButton"> True if it is a sub button. </param>
    private void buttonCallback(int index, bool isSubButton) {
        if(!isSubButton) {
            // If a main button was clicked...
            ActionButton clickedButton = this.currentlyShownButtons[index];
            if(clickedButton is ActionButtonParent) {
                // Clicked on a parent button conatining sub buttons.

                // Disable all the main buttons except for the selected one.
                for(int i = 0; i < MAX_BUTTONS; i++) {
                    this.buttonWrappers[i].setInteractable(this.isPoupShown() || i == index);
                }

                // Set the sub button text and set them to be active.
                ActionButton[] subButtons = ((ActionButtonParent)clickedButton).getChildButtons();
                for(int i = 0; i < MAX_BUTTONS; i++) {
                    if(i < subButtons.Length && !this.isPoupShown()) {
                        this.subButtonWrappers[i].setText(subButtons[i].getText());
                        this.subButtonWrappers[i].setVisible(true);
                    }
                    else {
                        this.subButtonWrappers[i].setVisible(false);
                    }
                }

                // Shift the sub button canvas up or down.
                this.subButtonCanvas.position = new Vector3(this.subButtonCanvas.position.x, this.buttonWrappers[index].button.transform.position.y + 20, 0);

                if(this.selectedMainButton == null) {
                    this.selectedMainButton = clickedButton;
                } else {
                    this.selectedMainButton = null;
                }
            }
            else {
                // Clicked on a normal button.
                this.callFunctionOnSelected(clickedButton);
            }
        }
        else {
            // If a sub button was clicked...
            ActionButton[] subButtons = ((ActionButtonParent)this.selectedMainButton).getChildButtons();

            this.closePopupButtons();
            this.callFunctionOnSelected(subButtons[index]);
        }

        this.updateSideButtons();
    }

    /// <summary>
    /// Cancels the delayed action.  This will enable the action buttons as well.
    /// </summary>
    public void cancelDelayedAction() {
        this.delayedButtonRef = null;
        this.setForceDisabled(false);

        // Remove the outline from all of the entities.
        foreach(SidedEntity entity in this.selectedOutlineObjects) {
            if(Util.isAlive(entity)) {
                entity.setOutlineVisibility(false, EnumOutlineParam.ACTION_OPTION);
            }
        }
        this.selectedOutlineObjects.Clear();
    }

    /// <summary>
    /// Calls the function of the passed ActionButton on whatever is selected (troop or building).
    /// </summary>
    private void callFunctionOnSelected(ActionButton actionButton) {
        if(actionButton is ActionButtonRequireClick) {
            ActionButtonRequireClick btnRequireClick = (ActionButtonRequireClick)actionButton;

            // Iterate through all objects and outline the ones that are valid selections.
            foreach(SidedEntity entity in Map.instance.findMapObjects(EntitySelecter.isBuilding)) {
                if(btnRequireClick.isValidForAction(this.player.getTeam(), entity)) {
                    entity.setOutlineVisibility(true, EnumOutlineParam.ACTION_OPTION);
                    this.selectedOutlineObjects.Add(entity);
                }
            }

            // If valid objects were found, store this action to be delayed
            if(this.selectedOutlineObjects.Count > 0) {
                this.delayedButtonRef = btnRequireClick;
                this.setForceDisabled(true);
            }

            // TODO tell the user they need to click something.  Outline valid selections?
        }
        else {
            // Do something right away.
            this.player.getSelected().callFunctionOn(actionButton);
        }
    }

    /// <summary>
    /// Used by the constructor to make a list of buttons.  This list is then returned.
    /// </summary>
    private ButtonWrapper[] makeButtons(Transform parent, bool isSub) {
        ButtonWrapper[] buttonList = new ButtonWrapper[MAX_BUTTONS];

        for(int i = 0; i < MAX_BUTTONS; i++) {
            GameObject btn = GameObject.Instantiate(this.buttonPrefab, parent);
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(68, -((40 * i) + 20), 0);
            btn.name = (isSub ? "Sub" : "Action") + "Button[" + i + "]";

            Button b = btn.GetComponent<Button>();
            int j = i;
            bool flag = isSub;
            b.onClick.AddListener(() => { this.buttonCallback(j, flag); });

            btn.SetActive(false);
            buttonList[i] = new ButtonWrapper(btn.GetComponent<Button>());
        }
        return buttonList;
    }

    private void checkButtonInteractability(ActionButton actionBtn, ButtonWrapper btnWrapper) {
        if(this.player.selectedBuilding.isSelected()) {
            btnWrapper.setInteractable(!actionBtn.shouldDisable(this.player.selectedBuilding.getBuilding()));
        }
        else {
            bool notInteractable = true;
            int buttonMask = actionBtn.getMask();
            foreach(UnitBase entity in this.player.selectedParty.getAllUnits()) {
                if((entity.getButtonMask() & buttonMask) != 0) {
                    notInteractable &= actionBtn.shouldDisable(entity) || !entity.isTaskCancelable();
                }
            }
            btnWrapper.setInteractable(!notInteractable);
        }
    }

    public ActionButtonRequireClick getDelayedActionButton() { //TODO rename.
        return this.delayedButtonRef;
    }
}
