public class MessageShowAnnouncement : AbstractMessageClient {

    public string message;
    public float duration;

    public MessageShowAnnouncement() { }

    public MessageShowAnnouncement(string message, float duration) {
        this.message = message;
        this.duration = duration;
    }

    public override short getID() {
        return 2001;
    }

    public override void processMessage(NetHandlerClient handler) {
        handler.showAnnouncement(this);
    }
}
