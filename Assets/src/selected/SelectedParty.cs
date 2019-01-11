using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectedParty : SelectedDisplayerBase {

    private const int PARTY_SIZE = 12;

    public GameObject buttonPrefab;

    [SerializeField] // For debugging in inspector
    private List<UnitBase> unitsInParty;
    private PartyButton[] partyButtons;

    public override void init(Player player) {
        base.init(player);

        this.unitsInParty = new List<UnitBase>();
        this.partyButtons = new PartyButton[PARTY_SIZE];

        for(int i = 0; i < PARTY_SIZE; i++) {
            GameObject btn = GameObject.Instantiate(this.buttonPrefab, this.transform);
            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(-((rt.sizeDelta.y * i) + 36), 36, 0);
            btn.name = "PartyButton[" + i + "]";

            PartyButton pb = btn.GetComponent<PartyButton>();
            pb.setIndex(i);

            this.partyButtons[i] = pb;
        }
    }

    // Hacky method.
    public override void onUpdate() {
        for(int i = this.unitsInParty.Count - 1; i >= 0; i--) {
            UnitBase unit = this.unitsInParty[i];
            if(!Util.isAlive(unit)) {
                this.remove(unit);
            }
        }
    }

    public override int getMask() {
        int mask = 0;
        foreach(UnitBase unit in this.unitsInParty) {
            mask |= unit.getButtonMask();
        }
        return mask;
    }

    [ClientSideOnly]
    public override void callFunctionOn(ActionButton actionButton) {
        this.player.callActionButton(actionButton, this.getAllUnits().Cast<SidedEntity>().ToList());
    }

    public override void clearSelected() {
        foreach(UnitBase unit in this.unitsInParty) {
            if(Util.isAlive(unit)) {
                unit.setOutlineVisibility(false, EnumOutlineParam.SELECTED);
            }
        }
        this.unitsInParty.Clear();
        foreach(PartyButton pb in this.partyButtons) {
            pb.setUnit(null);
        }
        this.hideIfEmpty();
    }

    /// <summary>
    /// Moves all members of the party to the passed destination.  Returns true if any units were moved.
    /// </summary>
    public bool moveAllTo(Vector3 point) {
        bool flag = false;
        int partySize = this.unitsInParty.Count;
        foreach(UnitBase unit in this.unitsInParty) {
            this.player.sendMessageToServer(new MessageSetUnitDestination(unit, point, partySize));
            flag = true;
        }
        return flag;
    }

    /// <summary>
    /// Returns the Unit at the passed index, or null if the index is out of bounds.
    /// </summary>
    public UnitBase getUnit(int index) {
        if(index < this.unitsInParty.Count) {
            return this.unitsInParty[index];
        }
        else {
            return null;
        }
    }

    /// <summary>
    /// Returns the number of Units in the party.
    /// </summary>
    public int getPartySize() {
        return this.unitsInParty.Count;
    }

    /// <summary>
    /// Returns true if the party is full and it can't hold any more members.
    /// </summary>
    public bool isFull() {
        return this.getPartySize() >= PARTY_SIZE;
    }

    /// <summary>
    /// Removes the passed Unit from the party if it's in the party.
    /// </summary>
    public void remove(UnitBase unit) {
        int index = this.unitsInParty.IndexOf(unit);
        if(index != -1) { // Make sure the Unit is actually in the party.
            this.partyButtons[index].setUnit(null);
            this.unitsInParty.RemoveAt(index);

            if(Util.isAlive(unit)) {
                unit.setOutlineVisibility(false, EnumOutlineParam.SELECTED);
            }

            // Slide the units down so there isn't an empty spot.
            for(int i = index; i < PARTY_SIZE; i++) {
                this.partyButtons[i].setUnit(this.getUnit(i));
            }
        }

        this.hideIfEmpty();
    }

    /// <summary>
    /// Returns a list containing off the Units in this party.
    /// </summary>
    public List<UnitBase> getAllUnits() {
        return this.unitsInParty;
    }

    /// <summary>
    /// Tries to add a Unit to the party.
    /// Returns false if the party is full and the Unit can't be added or if the Unit is already in the party.
    /// </summary>
    public bool tryAdd(UnitBase unit) {
        if(!this.isFull() && !this.unitsInParty.Contains(unit)) {
            this.unitsInParty.Add(unit);
            unit.setOutlineVisibility(true, EnumOutlineParam.SELECTED);
            this.partyButtons[this.unitsInParty.Count - 1].setUnit(unit);

            this.hideIfEmpty();

            return true;
        }
        else {
            return false;
        }
    }

    public void onButtonLeftClick(int index) {
        UnitBase unit = this.getUnit(index);
        if(unit != null) {
            this.player.centerCameraOn(unit.transform.position);
        }
    }

    public void onButtonRightClick(int index) {
        UnitBase unit = this.getUnit(index);
        this.remove(unit);
    }

    /// <summary>
    /// Returns true if the passed Unit is a memeber of this party.
    /// </summary>
    public bool contains(UnitBase unit) {
        foreach(UnitBase u in this.unitsInParty) {
            if(u == unit) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Hides the hud if the party is empty, or reveals it if it has as least one member.
    /// </summary>
    private void hideIfEmpty() {
        this.setUIVisible(this.unitsInParty.Count != 0);
    }
}