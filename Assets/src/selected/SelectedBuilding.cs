using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBuilding : SelectedDisplayerBase {

    private BuildingBase selected;
    private Text infoText;
    private Text otherText;

    public QueuedProducerStatusIcon[] icons;

    public override void init(Player player) {
        base.init(player);

        Text[] texts = this.GetComponentsInChildren<Text>();
        this.infoText = texts[0];
        this.otherText = texts[1];
    }

    public override void onUpdate() {
        if(this.selected != null) {
            string s = this.selected.getHealth() + "/" + this.selected.getMaxHealth() + (this.selected.isConstructing() ? " (Building)" : string.Empty);
            this.infoText.text = this.selected.getData().getName() + "\n" + s;

            if(this.selected is BuildingQueuedProducerBase) {
                BuildingQueuedProducerBase producer = (BuildingQueuedProducerBase)this.selected;

                EntityBaseStats ed;
                float trainTime = 0;
                int queueCount = producer.trainingQueue.Count;
                for(int i = 0; i < 3; i++) {
                    if(i < queueCount) {
                        ed = producer.trainingQueue[i].getPrefab().GetComponent<UnitBase>().getData();
                        if(i == 0) {
                            trainTime = ed.getProductionTime();
                        }
                        this.icons[i].setText(ed.getUnitTypeName());
                    }
                    else {
                        this.icons[i].setText(null);
                    }
                }

                this.otherText.text = queueCount == 0 ? "Empty" : Mathf.Floor(trainTime - producer.getTrainingProgress()) + 1 + " Seconds";
            }
            else if(this.selected is IResourceHolder) {
                IResourceHolder holder = (IResourceHolder)this.selected;
                int held = holder.getHeldResources();
                int limit = holder.getHoldLimit();
                string color = held >= limit ? "red" : "black";
                this.otherText.text = "<color=" + color + ">Storage:\n" + held + "/" + limit + "</color>";
            }
            else {
                this.otherText.text = string.Empty;
            }
        }
    }

    public override int getMask() {
        return this.selected.getButtonMask();
    }

    [ClientSideOnly]
    public override void callFunctionOn(ActionButton actionButton) {
        List<SidedEntity> list = new List<SidedEntity>(1);
        list.Add(this.selected);
        this.player.callActionButton(actionButton, list);
    }

    public override void clearSelected() {
        if(this.selected != null) {
            this.selected.setOutlineVisibility(false, EnumOutlineParam.SELECTED);
        }
        this.selected = null;
        this.setUIVisible(false);
    }

    public void setSelected(BuildingBase entity) {
        this.clearSelected();

        if(entity != null) {
            this.selected = entity;
            this.selected.setOutlineVisibility(true, EnumOutlineParam.SELECTED);
        }

        this.setUIVisible(this.selected != null);

        // UI setup for specific buildings.
        if(this.selected is BuildingQueuedProducerBase) {
            BuildingQueuedProducerBase producer = (BuildingQueuedProducerBase)this.selected;
            int slots = producer.getQueueSize();
            for(int i = 0; i < 3; i++) {
                this.icons[i].setVisible(i < slots);
            }
        }
        else {
            for(int i = 0; i < this.icons.Length; i++) {
                this.icons[i].setVisible(false);
            }
        }
    }

    /// <summary>
    /// Returns the selected building, may be null.
    /// </summary>
    public BuildingBase getBuilding() {
        return this.selected;
    }

    /// <summary>
    /// Returns true if a building is selected.
    /// </summary>
    public bool isSelected() {
        return this.selected != null;
    }
}