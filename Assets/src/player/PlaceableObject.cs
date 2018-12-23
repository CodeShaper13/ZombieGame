public class PlaceableObject {

    public string displayText;
    public RegisteredObject registeredObject;
    private int count;

    public PlaceableObject(string s, RegisteredObject r, int count) {
        this.displayText = s;
        this.registeredObject = r;
        this.count = count;
    }

    public int getCount() {
        return this.count;
    }

    public void setCount(int count) {
        this.count = count;
    }
}
