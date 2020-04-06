using UnityEngine;

public class DoorDoubleCaller : MonoBehaviour, IClickable<IPlayer> {

    private DoorDouble doubleDoor;

    private void Awake() {
        this.doubleDoor = this.GetComponentInParent<DoorDouble>();
    }

    public bool onClick(IPlayer player) {
        return this.doubleDoor.onClick(player);
    }
}
