using B83.Unity.Attributes;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour {

    [Tooltip("Enable if the ragdoll has a general collider while it's \"alive\", and specific ones should be anabled when it's floppy.")]
    public bool hasGeneralCollider;
    public bool generateRagdollPartComponenets;
    [Tooltip("Script to put on all child objects.  It must extend RagdollPart")]
    [MonoScript(type = typeof(RagdollPart))]
    public string script;

    private RagdollPart[] parts;

    private void Awake() {
        if(this.generateRagdollPartComponenets) {
            // Add RagdollPart components to all of the pieces.
            foreach(Rigidbody rb in this.GetComponentsInChildren<Rigidbody>()) {
                if(rb.transform != this.transform) { // Ignore if there is a RB on this object, only look at children.
                    RagdollPart part = rb.gameObject.GetComponent<RagdollPart>();
                    if(part == null) {
                        // Add a script
                        if(this.script != null) {
                            // Add the user defined component.
                            rb.gameObject.AddComponent(Type.GetType(this.script));
                        } else {
                            // Add the basic component.
                            rb.gameObject.AddComponent<RagdollPart>();
                        }
                    }
                }
            }
        }

        this.parts = this.GetComponentsInChildren<RagdollPart>();

        if(this.hasGeneralCollider) {
            // Disable all of the more specific colliders.
            foreach(RagdollPart part in this.parts) {
                part.colliderComponent.enabled = false;
            }
        }
    }

    /// <summary>
    /// Makes the Regdoll floppy.  disableNavAgent and disableAnimator will disable the respective components on the object this script is attached to if they exist.
    /// </summary>
    public void makeFloppy(bool disableNavAgent = false, bool disableAnimator = false) {
        if(disableNavAgent) {
            NavMeshAgent agent = this.GetComponent<NavMeshAgent>();
            if(agent != null) {
                agent.enabled = false;
            }
        }

        if(disableAnimator) {
            Animator anim = this.GetComponent<Animator>();
            if(anim != null) {
                anim.enabled = false;
            }
        }

        if(this.hasGeneralCollider) {
            // Disable the collider attached to the root object.
            Collider col = this.GetComponent<Collider>();
            if(col != null) {
                col.enabled = false;
            }
        }

        foreach(RagdollPart part in this.parts) {
            part.rigidbodyComponent.isKinematic = false;

            part.onBecomeFloppy();

            if(this.hasGeneralCollider) {
                // Enable the part specific colliders.
                Collider col = part.gameObject.GetComponent<Collider>();
                if(col != null) {
                    col.enabled = true;
                }
            }
        }
    }
}
