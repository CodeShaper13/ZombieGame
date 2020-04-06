using System;
using UnityEngine;

public class MapSP : MapBase {

    private CampaignLevelData campaignData;
    private GameModeBase gameMode;

    public override void initialize(MapData mapData) {
        base.initialize(mapData);

        this.gameMode = GameObject.FindObjectOfType<GameModeBase>();
        if(this.gameMode == null) {
            throw new Exception("No GameObject with a component of GameModeBase or a derived type could be located!");
        }
        this.gameMode.init(this);

        this.setResources(Team.SURVIVORS_1, this.campaignData.startingResources);
    }

    protected override void updateMap() {
        this.gameMode.updateGameMode();
    }

    public void setCampaignData(CampaignLevelData cld) {
        this.campaignData = cld;
    }

    public override int getPlayerCount() {
        return 1;
    }

    public CampaignLevelData getCampaignData() {
        return this.campaignData;
    }
}
