using UnityEngine;

public class ScrollableInt {

    private int min;
    private int max;
    private int index;

    public ScrollableInt(int inclusiveMin, int inclusiveMax) {
        this.min = inclusiveMin;
        this.max = inclusiveMax;
    }

    public int get() {
        return this.index;
    }

    /// <summary>
    /// Clamps the value of i between the min and max.
    /// </summary>
    public void set(int i) {
        this.index = Mathf.Clamp(i, this.min, this.max);
    }

    public int getMin() {
        return this.min;
    }

    public int getMax() {
        return this.max;
    }

    /// <summary>
    /// Scrolls in the passed direction.  1 = up, -1 = down, 0 = dont't move.
    /// </summary>
    public void scroll(int scrollDirection) {
        scrollDirection = Mathf.Clamp(scrollDirection, -1, 1);

        int newIndex = this.index + scrollDirection;
        if(newIndex > this.max) {
            newIndex = this.min;
        }
        else if(newIndex < this.min) {
            newIndex = this.max;
        }
        this.index = newIndex;
    }
}
