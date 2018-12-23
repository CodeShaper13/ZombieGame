using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyButton : MonoBehaviour, IPointerClickHandler {

    private SelectedParty party;
    private int index;
    private UnitBase unit;
    private Text btnHpText;
    [SerializeField]
    private Button infoButton;

    private void Awake() {
        this.party = this.GetComponentInParent<SelectedParty>();
        this.btnHpText = this.GetComponentInChildren<Text>();
        this.infoButton = this.transform.GetChild(1).GetComponent<Button>();

        this.setUnit(null);
    }

    private void Update() {
        if(this.unit != null) {
            string s = this.unit.getHealth() + "/" + this.unit.getMaxHealth();
            this.btnHpText.text = this.unit.getData().getUnitTypeName() + "\n" + s;
        }
        else {
            this.btnHpText.text = "...";
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) {
            this.party.onButtonLeftClick(this.index);
        }
        else if(eventData.button == PointerEventData.InputButton.Middle) {

        }
        else if(eventData.button == PointerEventData.InputButton.Right) {
            this.party.onButtonRightClick(this.index);
        }
    }

    public void setUnit(UnitBase unit) {
        this.unit = unit;
        this.infoButton.gameObject.SetActive(unit != null);
    }

    public void setIndex(int i) {
        this.index = i;
    }

    /// <summary>
    /// Called when the info button is clicked.
    /// </summary>
    public void callback_infoButton() {
        print("TODO");
        //GuiScreenUnitStats guiStats = (GuiScreenUnitStats)GuiManager.openGui(GuiManager.unitStats);
        //guiStats.set(this.party.getUnit(this.index));
    }
}