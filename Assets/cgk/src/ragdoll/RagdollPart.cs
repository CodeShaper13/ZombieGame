using UnityEngine;
using System;

public class RagdollPart : MonoBehaviour {

    [NonSerialized]
    public Rigidbody rigidbodyComponent;
    [NonSerialized]
    public Collider colliderComponent;
    [NonSerialized]
    protected Ragdoll ragdoll;

    private void Awake() {
        this.rigidbodyComponent = this.GetComponent<Rigidbody>();
        this.colliderComponent = this.GetComponent<Collider>();
        this.ragdoll = this.GetComponentInParent<Ragdoll>();

        this.rigidbodyComponent.isKinematic = true;
    }

    /// <summary>
    /// Called when the Ragdoll becomes floppy.
    /// </summary>
    public virtual void onBecomeFloppy() { }
}