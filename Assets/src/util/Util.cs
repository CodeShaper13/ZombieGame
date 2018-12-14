public static class Util {

    /// <summary>
    /// Returns true if the passed LivingObject is both not null and alive.
    /// </summary>
    public static bool isAlive(LivingObject obj) {
        return obj != null && !obj.isDead();
    }
}
