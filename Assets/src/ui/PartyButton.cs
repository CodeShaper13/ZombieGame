using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyButton : MonoBehaviour, IPointerClickHandler {

    private SelectedParty party;
    private int index;
    private UnitBase unit;
    private Text btnHpText;

    private void Awake() {
        this.party = this.GetComponentInParent<SelectedParty>();
        this.btnHpText = this.GetComponentInChildren<Text>();

        this.setUnit(null);
    }

    private void Update() {
        if(this.unit != null) {
            string s = this.unit.getHealth() + "/" + this.unit.getMaxHealth();
            this.btnHpText.text = this.unit.unitData.unitName + "\n" + s;
        }
        else {
            this.btnHpText.text = "...";
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(this.unit != null) {
            if(eventData.button == PointerEventData.InputButton.Left) {
                // Center camera on.
                Player.localPlayer.centerCameraOn(this.unit.transform.position);
            }
            else if(eventData.button == PointerEventData.InputButton.Middle) {
                // Show unit info.
                Player.localPlayer.sendMessageToServer(new MessageRequestStats(this.party.getUnit(this.index)));
            }
            else if(eventData.button == PointerEventData.InputButton.Right) {
                // Deselect.
                this.party.remove(this.unit);
            }
        }
    }

    public void setUnit(UnitBase unit) {
        this.unit = unit;
    }

    public void setIndex(int i) {
        this.index = i;
    }
}