/// <summary>
/// Interface for Objects that can interact with a door.
/// </summary>
public interface IDoorInteractor {

    void onDoorAreaEnter(DoorBase door);

    void onDoorAreaLeave(DoorBase door);
}