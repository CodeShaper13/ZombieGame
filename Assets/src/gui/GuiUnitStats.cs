using UnityEngine;
using UnityEngine.UI;

public class GuiUnitStats : GuiBase {

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text statText;

    public override void onGuiInit() { }

    public void set(MessageShowStatsGui msg) {
        this.nameText.text = msg.nameString;
        this.statText.text = msg.statsString;
        //stats.dirty = false;
    }
}
