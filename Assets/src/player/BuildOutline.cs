using System.Collections.Generic;
using UnityEngine;

public class BuildOutline : MonoBehaviour {

    private const float HEIGHT = 0.1f;

    private Player player;

    [SerializeField]
    private Material invalidMaterial;
    [SerializeField]
    private Material validMaterial;

    private MeshRenderer meshRenderer;

    private RegisteredObject buildingToPlace;
    private List<UnitBuilder> cachedBuilders;

    public void init(Player player) {
        this.player = player;

        this.cachedBuilders = new List<UnitBuilder>();
        this.meshRenderer = this.GetComponent<MeshRenderer>();

        // Make sure this object is at the right height.
        this.transform.position.setY(HEIGHT);

        this.disableOutline();
    }

    private void Update() {
        this.moveSquare();
        this.updateColor();
    }      

    private void moveSquare() {
        RaycastHit hit;
        Vector3 correctedHit = Vector3.zero;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            correctedHit = new Vector3(
                Mathf.Round(hit.point.x),
                HEIGHT,
                Mathf.Round(hit.point.z));
            this.transform.position = correctedHit;
        }
    }

    private void updateColor() {
        this.meshRenderer.material = this.isSpaceFree() ? this.validMaterial : this.invalidMaterial;
    }

    /// <summary>
    /// Returns true if the build outline is enabled outline specific input should be
    /// handled instead of normal clicking.
    /// </summary>
    public bool isEnabled() {
        return this.gameObject.activeSelf;
    }

    public void handleClick() {
        if(Input.GetMouseButtonUp(0) && this.isSpaceFree()) {
            // Pick the builder to use.
            UnitBuilder builder = Util.closestToPoint(this.transform.position, this.cachedBuilders, (entity) => {
                return entity.isTaskCancelable();
            });

            if(builder != null) {
                this.player.sendMessageToServer(new MessageConstructBuilding(
                    this.player.getTeam(),
                    this.buildingToPlace,
                    new Vector3(this.transform.position.x, 0, this.transform.position.z),
                    builder));

                this.disableOutline();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            this.disableOutline();
        }
    }

    /// <summary>
    /// Called to enable the outline effect.
    /// </summary>
    public void enableOutline(RegisteredObject registeredBuilding, UnitBuilder builder) {
        // Note, this could be called multiple times if multiple builders are in the same party.
        this.gameObject.SetActive(true);

        this.buildingToPlace = registeredBuilding;
        this.cachedBuilders.Add(builder);

        this.setSize(this.buildingToPlace.getPrefab().GetComponent<BuildingBase>());

        // Gray out the action buttons while the outline is being shown.
        Player.localPlayer.actionButtons.setForceDisabled(true);
    }

    /// <summary>
    /// Disables the outline effect.
    /// </summary>
    public void disableOutline() {
        this.gameObject.SetActive(false);
        this.cachedBuilders.Clear();
        Player.localPlayer.actionButtons.setForceDisabled(false);
    }

    /// <summary>
    /// Sets the size of the outline base effect.
    /// </summary>
    private void setSize(BuildingBase building) {
        Vector2 scale = building.getFootprintSize();
        this.transform.localScale = new Vector3(scale.x - 0.1f, scale.y - 0.1f, 0.01f);
    }

    private bool isSpaceFree() {
        return !Physics.CheckBox(this.transform.position, this.transform.lossyScale / 2, this.transform.rotation, Layers.GEOMETRY);
    }
}