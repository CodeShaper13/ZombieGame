using UnityEngine;
using UnityEngine.UI;

public class GameModeEndless : GameModeBase {

    private int currentWave;

    [Header("DO NOT CHANGE!")]
    [SerializeField]
    private Text textCurrentWaveCount;
    [SerializeField]
    private Text textWaveAnnouncement;

    public override void updateGameMode() {
        base.updateGameMode();
    }

    protected override string getDescription() {
        throw new System.NotImplementedException();
    }

    protected override string getGameModeName() {
        return "Endless";
    }
}
