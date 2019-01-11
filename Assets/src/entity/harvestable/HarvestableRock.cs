public class HarvestableRock : HarvestableObject {

    public override float getHealthBarHeight() {
        return 1.5f;
    }

    public override string getDisplayName() {
        return "Rock";
    }

    protected override int getTotalResourceYield() {
        return Constants.HARVESTABLE_ROCK_HEALTH;
    }
}
