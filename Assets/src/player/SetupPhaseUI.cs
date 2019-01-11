using UnityEngine;
using UnityEngine.UI;

public class SetupPhaseUI : MonoBehaviour {

    private const int MAX_BUTTONS = 9;

    /// <summary>
    /// -1 means nothing is selected.
    /// </summary>
    private int selected = -1;
    private UIPlaceObjectButton[] buttonList;

    // Use this for initialization
    private void Awake() {
        PlaceableObject[] objs = new PlaceableObject[] {
            new PlaceableObject(Registry.unitBuilder),
            new PlaceableObject(Registry.unitArcher),
            new PlaceableObject(Registry.buildingCamp),
            new PlaceableObject(Registry.buildingFlag),
            new PlaceableObject(Registry.buildingProducer),
            new PlaceableObject(Registry.buildingStoreroom),
            new PlaceableObject(Registry.buildingCannon),
            new PlaceableObject(Registry.buildingTrainingHouse),
            new PlaceableObject(Registry.buildingWorkshop)
        };

        // Make the buttons.
        this.buttonList = new UIPlaceObjectButton[MAX_BUTTONS];
        for (int i = 0; i < MAX_BUTTONS; i++) {
            GameObject btn = GameObject.Instantiate(References.list.prefabPlaceObjectButton, this.transform);
            const int k = 32 + 16;
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(k + ((64+12) * i), k, 0);
            btn.name = "Button[" + i + "]";

            Button b = btn.GetComponent<Button>();
            int j = i;
            b.onClick.AddListener(() => { this.buttonCallback(j); });

            PlaceableObject po = objs[i];
            btn.SetActive(po != null);
            this.buttonList[i] = new UIPlaceObjectButton(btn.GetComponent<Button>(), po);
        }

        this.buttonCallback(0);
    }

    private void Update() {
        foreach(UIPlaceObjectButton obj in this.buttonList) {
            if(obj.placeableObj != null) {
                int i = obj.placeableObj.getCount();
                obj.setText(obj.placeableObj.displayText + (i == -1 ? string.Empty :" x" + obj.placeableObj.getCount()));
            }
        }
    }

    /// <summary>
    /// Returns null if nothing is selected.
    /// </summary>
    public PlaceableObject getSelectedObject() {
        if(this.selected == -1) {
            return null;
        }
        PlaceableObject placeableObject = this.buttonList[this.selected].placeableObj;
        if(placeableObject == null) {
            return null;
        } else {
            return placeableObject;
        }
    }

    public void func(int index, bool flag) {
        this.buttonList[index].setInteractable(flag);
    }

    private void buttonCallback(int buttonIndex) {
        this.func(1f);

        if (buttonIndex == this.selected) {
            this.selected = -1;
        } else {
            this.selected = buttonIndex;
        }

        this.func(1.25f);
    }

    private void func(float f) {
        if (this.selected != -1) {
            this.buttonList[this.selected].setSize(f);
        }
    }
}
