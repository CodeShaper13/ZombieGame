public interface ITask : IDrawDebug, INbtSerializable {

    /// <summary>
    /// Preforms the AI task.  Return true to keep running this task on the next tick.
    /// </summary>
    bool preform(float deltaTime);

    /// <summary>
    /// Called whenever the task ends.  This may be from preform returning
    /// true or a different task was started.
    /// </summary>
    void onFinish();

    void onDamage(MapObject damager);
}
