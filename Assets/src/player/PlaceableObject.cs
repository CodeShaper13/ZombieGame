public class PlaceableObject {

    public string displayText;
    public RegisteredObject registeredObject;
    private int count;

    public PlaceableObject(RegisteredObject r, int count = -1) {
        this.displayText = r.getPrefab().GetComponent<MapObject>().getDisplayName();
        this.registeredObject = r;
        this.count = count;
    }

    public int getCount() {
        return this.count;
    }

    public void setCount(int count) {
        if(this.count != -1) {
            this.count = count;
        }
    }
}
