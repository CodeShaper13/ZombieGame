using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBase : LivingObject {

    private NavMeshAgent agent;

    /// <summary> The transfrom of the head object. </summary>
    public Transform head;

    private SidedEntity closest;
    public GunBase weapon;

    public override void onAwake() {
        base.onAwake();

        this.agent = this.GetComponent<NavMeshAgent>();

        if (this.weapon != null) {
            //this.weapon = GameObject.Instantiate(this.weapon);
            //this.weapon.transform.parent = this.head;
            this.weapon.setHoldLocation(this);
        }
    }

    public override void onUpdate() {
        base.onUpdate();

        this.closest = this.getClosestEnemyObject();

        bool isTargetVisible = (this.closest != null && this.canSeeTarget(this.closest));

        // Rotate head to face target, or ahead if there is none.
        if (isTargetVisible) {
            this.head.LookAt(this.closest.transform.position); // Quaternion.Slerp(this.head.rotation, rot, 10 * Time.deltaTime); //Smooth rot makes bullet firing difficult...
        }
        else {
            this.head.rotation = this.transform.rotation;
        }

        // Behavior.
        if (isTargetVisible && this.weapon.isInRange(this.closest)) {
            // We can see the enemy and our weapon is in range, stand still and fire.
            this.weapon.tryFire();
            this.agent.SetDestination(this.transform.position);
        }
        else {
            if (this.closest != null) {
                // Move towards the enemy to get within weapon range and/or to get them in line of sight.
                this.agent.SetDestination(this.closest.transform.position);
            }
            else {
                this.agent.SetDestination(this.transform.position);
            }
        }

        // Update/use weapon.
        this.weapon.updateGun();
    }

    public override Vector3 getFootPos() {
        return this.transform.position - new Vector3(0, -1, 0);
    }

    public bool canSeeTarget(SidedEntity target) {
        RaycastHit hit;
        Transform sightTrans = this.head; // Change to getter?
        //int layerMask = ~(1 << 8);
        //Soldier.setLayer(this.gameObject, 8);
        if (Physics.Linecast(sightTrans.position, target.transform.position, out hit)) { //, layerMask)) {
            UnitSoldier unit = hit.transform.GetComponent<UnitSoldier>();
            if (unit != null && unit.getTeam() != this.getTeam()) {
                this.debugLine(sightTrans.position, hit.point, 1);
                return true;
            }
            else {
                this.debugLine(sightTrans.position, hit.point, 0);
                return false;
            }
        }
        else {
            this.debugLine(sightTrans.position, sightTrans.forward * 100, -1);
            return false;
        }
    }

    //-1 = red, 0 = yellow, 1 = green
    private void debugLine(Vector3 start, Vector3 end, int color) {
        if (true) {
            Debug.DrawLine(start, end, color < 0 ? Color.red : color == 0 ? Color.yellow : Color.green);
        }
    }
}
