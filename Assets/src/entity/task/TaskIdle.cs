using UnityEngine;

public class TaskIdle : TaskBase<UnitBase>, ICancelableTask {

    public TaskIdle(UnitBase unit) : base(unit) { }

    public override bool preform(float deltaTime) {
        return true;
    }

    // TODO is not called
    public override void onDamage(MapObject dealer) {
        // If a unit is damaged while idling, attack whatever damaged it.
        Debug.Log("hit!");
        if (dealer is Projectile) {
            SidedEntity shooter = ((Projectile)dealer).getShooter();
            if (this.unit.getTeam() != shooter.getTeam()) {
                this.setToAttack(shooter);
            }
        }
        else if (dealer is SidedEntity) {
            SidedEntity attacker = (SidedEntity)dealer;
            if (this.unit.getTeam() != attacker.getTeam()) {
                this.setToAttack(attacker);
            }
        }
    }

    private void setToAttack(LivingObject target) {
        Debug.Log("changing task");
        this.unit.setTask(new TaskAttackNearby(this.unit, target));
    }
}
