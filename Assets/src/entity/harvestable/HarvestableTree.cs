public class HarvestableTree : HarvestableObject {

    protected override int getTotalResourceYield() {
        return Constants.HARVESTABLE_TREE_HEALTH;
    }

    public override float getHealthBarHeight() {
        return this.transform.localScale.y * 6.8f;
    }

    public override string getDisplayName() {
        return "Tree";
    }
}
