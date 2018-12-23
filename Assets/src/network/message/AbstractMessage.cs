using UnityEngine.Networking;

public abstract class AbstractMessage<T> : MessageBase where T : NetHandlerBase {

    public AbstractMessage() { }

    public abstract void processMessage(T handler);

    /// <summary>
    /// Returns a unique ID for this message.
    /// </summary>
    public abstract short getID();
}
