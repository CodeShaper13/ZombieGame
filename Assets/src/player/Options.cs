using System;
using System.Collections.Generic;
using UnityEngine;

public class Options {

    public OptionFloat sensitivity = new OptionFloat("Sensitivity", 40f);
    public OptionFloat zoomSensitivity = new OptionFloat("Zoom Speed", 300f);

    private List<Option<object>> allOptions = new List<Option<object>>();

    public Options() {
        this.allOptions = new List<Option<object>>();

    }

    public void draw() {
        float f = 25;
        foreach(Option<object> option in this.allOptions) {
            option.draw(new Vector2(10, f));
            f += 25;
        }
    }

    public abstract class Option<T> {

        protected string displayName;
        protected T value;

        public Option(string displayName, T defaultValue) {
            this.displayName = displayName;
            this.set(defaultValue);
        }

        public T get() {
            return this.value;
        }

        public virtual void draw(Vector2 pos) {
            GUI.Label(new Rect(pos - new Vector2(10, 0), Vector2.one), this.displayName);
        }

        public abstract void set(T amount);

        public override string ToString() {
            return this.displayName + ": " + this.value.ToString();
        }
    }

    public class OptionInteger : Option<int> {

        public OptionInteger(string displayName, int defaultValue) : base(displayName, defaultValue) { }

        public override void draw(Vector2 pos) {
            GUI.TextField(new Rect(pos, Vector2.one), this.value.ToString());
        }

        public override void set(int value = 1) {
            this.value = value;
        }
    }

    public class OptionFloat : Option<float> {

        public OptionFloat(string displayName, float defaultValue) : base(displayName, defaultValue) { }

        public override void set(float value) {
            this.value = value;
        }

        public override void draw(Vector2 pos) {
            GUI.TextField(new Rect(pos, Vector2.one), this.value.ToString());
        }
    }

    public class OptionBool : Option<bool> {

        public OptionBool(string displayName, bool defaultValue) : base(displayName, defaultValue) { }

        public override void set(bool value) {
            this.value = value;
        }

        public override void draw(Vector2 pos) {
            GUI.Toggle(new Rect(pos, Vector2.one), this.value, this.displayName);
        }
    }
}
