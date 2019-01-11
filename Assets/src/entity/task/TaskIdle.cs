public class TaskIdle : TaskAttackNearby {

    public TaskIdle(UnitBase unit) : base(unit) { }

    public override bool preform(float deltaTime) {
        if (Util.isAlive(this.target)) {
            this.func();
        }
        else {
            this.target = null;
            this.moveHelper.stop();
        }

        return true;
    }

    public override void onDamage(MapObject dealer) {
        if (dealer is Bullet) {
            SidedEntity shooter = ((Projectile)dealer).getShooter();
            if (this.unit.getTeam() != shooter.getTeam()) {
                this.target = shooter;
            }
        }
        else if (dealer is SidedEntity) {
            SidedEntity entity = (SidedEntity)dealer;
            if (this.unit.getTeam() != entity.getTeam()) {
                this.target = entity;
            }
        }
    }

    protected override SidedEntity findTarget() {
        return null; // No target should be found, this is called in the constructor.
    }
}
