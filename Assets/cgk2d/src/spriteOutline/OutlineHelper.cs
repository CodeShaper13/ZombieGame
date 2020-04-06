using UnityEngine;
using System;

[RequireComponent(typeof(SpriteOutline))]
public class OutlineHelper : MonoBehaviour {

    [SerializeField]
    private OutlineType[] outlines;

    private SpriteOutline spriteOutline;

    private void Awake() {
        this.spriteOutline = this.GetComponent<SpriteOutline>();
    }

    public void setVisible(string name) {
        this.func(name, true);

        this.updateComponent();
    }

    public void setInvisible(string name) {
        this.func(name, false);

        this.updateComponent();
    }

    /// <summary>
    /// Hides all of the outlines.
    /// </summary>
    public void hideAll() {
        for(int i = 0; i < this.outlines.Length; i++) {
            this.outlines[i].visible = false;
        }

        this.updateComponent();
    }

    /// <summary>
    /// Returns true if an outline with the passed name is visible.
    /// If no outline exists with the passed name, false is returned.
    /// </summary>
    public bool isVisible(string name) {
        OutlineType oc = this.getEntry(name);
        if(oc != null) {
            return true;
        } else {
            return false;
        }
    }

    private void updateComponent() {
        bool keepComponenetEnabled = false;
        foreach(OutlineType ot in this.outlines) {
            if(ot.visible) {
                this.spriteOutline.setColor(ot.color);
                keepComponenetEnabled = true;
            }
        }

        this.spriteOutline.enabled = keepComponenetEnabled;
    }

    private void func(string s, bool b) {
        OutlineType oc = this.getEntry(s);
        if(oc != null) {
            oc.visible = b;
        }
    }

    private OutlineType getEntry(string s) {
        foreach(OutlineType oc in this.outlines) {
            if(oc.name == s) {
                return oc;
            }
        }
        return null;
    }

    [Serializable]
    public class OutlineType {

        public string name;
        public Color color;
        public bool visible;

        public OutlineType(string name, Color color) {
            this.name = name;
            this.color = color;
            this.visible = false;
        }
    }
}
