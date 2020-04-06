using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class CampaignNode : MonoBehaviour {

    [SerializeField]
    private LineRenderer lr;

    [Header("")]
    [SerializeField]
    private CampaignNode nextLevel;

    [SerializeField]
    private CampaignLevelData campaignData;

    private GuiCampaignSelect gui;

    private void Awake() {
        this.gui = this.GetComponentInParent<GuiCampaignSelect>();
    }

    private void Update() {
        if(Application.IsPlaying(gameObject)) {
            // Play logic
        } else {
            if(this.nextLevel == this) {
                throw new Exception("Can not link to the same node!");
            } else if(this.nextLevel != null) {
                this.lr.positionCount = 2;
                this.lr.SetPositions(new Vector3[] { this.transform.position, this.nextLevel.transform.position });
            } else {
                this.lr.positionCount = 0;
                this.lr.SetPositions(new Vector3[0]);
            }
        }
    }

    public void callback_onClick() {
        this.gui.loadSpWorld(this.campaignData);
    }
}
