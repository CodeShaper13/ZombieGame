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
            new PlaceableObject("Soldier", Registry.unitSoldier),
            new PlaceableObject("Archer", Registry.unitArcher),
            new PlaceableObject("Scout", Registry.unitScout),
            new PlaceableObject("Cannon", Registry.buildingCannon),
            new PlaceableObject("Wall", Registry.buildingWall),
            new PlaceableObject("Flag", Registry.buildingFlag),
            null,
            null,
            null
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

    public int getSelectedIndex() {
        return this.selected;
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
