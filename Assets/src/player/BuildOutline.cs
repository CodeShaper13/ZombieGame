using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildOutline : MonoBehaviour {

    private Player player;

    [SerializeField]
    private Color invalidLocationColor = Color.white;
    [SerializeField]
    private Color validLocationColor = Color.white;

    private SpriteRenderer spriteRenderer;
    private RegisteredObject buildingToPlace;
    private List<UnitBuilder> cachedBuilders;

    private void Awake() {
        this.player = this.GetComponentInParent<Player>();

        this.cachedBuilders = new List<UnitBuilder>();
        this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        this.disableOutline();
    }

    private void Update() {
        this.moveSquare();
        this.updateColor();
    }      

    private void moveSquare() {
        if(EventSystem.current.IsPointerOverGameObject()) {
            this.spriteRenderer.enabled = false;
        } else {
            Vector2Int pos = this.player.getMousePos();
            this.transform.position = new Vector3(pos.x - 0.5f, pos.y - 0.5f); // TODO remove decimals?

            if(TileMaps.singleton.objectMap.GetTile(new Vector3Int(pos.x, pos.y, 0)) == null) {

            }

            this.spriteRenderer.enabled = true;
        }

        /*
        RaycastHit hit;
        Vector3 correctedHit = Vector3.zero;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, Layers.GROUND)) {
            // Mouse is over the ground!

            ICustomBuildOutline customOutline = this.buildingToPlace.getPrefab().GetComponent<ICustomBuildOutline>();
            bool flag = false;
            if(customOutline != null) {
                flag = customOutline.func(new Vector3(hit.point.x, hit.point.y), this.transform);
            }

            if(!flag) {
                // Snap square to grid
                correctedHit = new Vector3(
                    Mathf.Round(hit.point.x),
                    Mathf.Round(hit.point.y));
                this.transform.position = correctedHit;
                this.transform.localScale = Vector3.one;
                this.transform.rotation = Quaternion.identity;
            }

            this.spriteRenderer.enabled = true;
        }
        else {
            this.spriteRenderer.enabled = false;
        }
        */
    }

    private void updateColor() {
        this.spriteRenderer.color = this.isSpaceFree() ? this.validLocationColor : this.invalidLocationColor;
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
                    new Vector3(this.transform.position.x, this.transform.position.y),
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
        this.player.actionButtons.setForceDisabled(true);
    }

    /// <summary>
    /// Disables the outline effect.
    /// </summary>
    public void disableOutline() {
        this.gameObject.SetActive(false);
        this.cachedBuilders.Clear();
        this.player.actionButtons.setForceDisabled(false);
    }

    /// <summary>
    /// Sets the size of the outline base effect.
    /// </summary>
    private void setSize(BuildingBase building) {
        Vector2 scale = building.getFootprintSize();
        this.transform.localScale = new Vector3(scale.x - 0.1f, scale.y - 0.1f, 0.01f);
    }

    private bool isSpaceFree() {
        return this.buildingToPlace.getPrefab().GetComponent<BuildingBase>().isValidLocation(this.transform.position);
    }
}