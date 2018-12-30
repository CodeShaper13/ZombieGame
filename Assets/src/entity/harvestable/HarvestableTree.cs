public class HarvestableTree : HarvestableObject {

    protected override int getTotalResourceYield() {
        return 150;
    }

    public override float getHealthBarHeight() {
        return this.transform.localScale.y * 6.8f;
    }
}
