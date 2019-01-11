public class MessageChangeGameState : AbstractMessageClient {

    public EnumGameState newState;

    public MessageChangeGameState() { }

    public MessageChangeGameState(EnumGameState newState) {
        this.newState = newState;
    }

    public override short getID() {
        return 2002;
    }

    public override void processMessage(NetHandlerClient handler) {
        handler.changeGameState(this);
    }
}
