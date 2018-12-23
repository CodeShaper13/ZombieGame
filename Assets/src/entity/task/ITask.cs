public interface ITask : IDrawDebug {

    /// <summary>
    /// Preforms the AI task.  Return true to keep running this task on the next tick.
    /// </summary>
    bool preform();

    /// <summary>
    /// Called whenever the task ends.  This may be from preform returning
    /// true or a different task was started.
    /// </summary>
    void onFinish();

    void onDamage(MapObject damager);

    /// <summary>
    /// Returns true if this Task can be canceld and a new one can be selected.
    /// </summary>
    bool cancelable();
}
