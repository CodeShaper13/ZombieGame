using System.Collections.Generic;

public class GuiCampaignSelect : GuiBase {

    private List<CampaignNode> nodes;

    public override void onGuiInit() {
        base.onGuiInit();

        this.nodes = new List<CampaignNode>();
        foreach(CampaignNode node in this.GetComponentsInChildren<CampaignNode>()) {
            this.nodes.Add(node);
        }
    }

    public override void onGuiOpen() {
        base.onGuiOpen();
    }

    public void loadSpWorld(CampaignLevelData campaignData) {
        CustomNetworkManager cnm = Main.getNetManager();
        cnm.isSinglePlayer = true;
        cnm.campaignData = campaignData;

        MapData data = new MapData("nul", 0);
        Main.loadScene(campaignData.sceneName, data, true);
    }
}
