using UnityEngine.Networking;

public abstract class AbstractMessage : MessageBase {

    public AbstractMessage() { }

    public abstract void processMessage(MessageProcesser ms);

    /// <summary>
    /// Returns a unique ID for this message.
    /// </summary>
    public abstract short getID();
}
