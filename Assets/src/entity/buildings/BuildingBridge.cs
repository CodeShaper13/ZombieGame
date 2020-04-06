using UnityEngine;
using UnityEngine.AI;

public class BuildingBridge : BuildingBase, ICustomBuildOutline {

    [SerializeField]
    private OffMeshLink meshLink;

    public override BuildingData getData() {
        return Constants.BD_BRIDGE;
    }

    public override Vector2 getFootprintSize() {
        return Vector3.one;
    }

    public override float getHealthBarHeight() {
        return 1f;
    }

    public override bool isValidLocation(Vector3 mousePos) {
        return !this.intersects(mousePos, Layers.GEOMETRY) && this.intersects(mousePos, Layers.BRIDGE_ZONE);
    }

    public override void onConstructionFinished(UnitBuilder unit) {
        base.onConstructionFinished(unit);

        this.meshLink.enabled = true;
    }

    public bool func(Vector3 point, Transform outlineTransform) {
        Collider[] c = Physics.OverlapSphere(point, 1f, Layers.BRIDGE_ZONE);
        if(c.Length > 0) {
            BridgeLocation bLoc = c[0].GetComponent<BridgeLocation>();
            if(bLoc != null) {
                outlineTransform.position = bLoc.getMidpoint();
                outlineTransform.rotation = bLoc.transform.rotation;
                outlineTransform.localScale = new Vector3(bLoc.getLength(), 1f, 1f);
                return true;
            }
            else {
                Logger.logError("A GameObject was found with the BridgeZone Layer, without a BridgeLocation Component.  This should never happen!");
            }
        }

        return false;
    }

    private bool intersects(Vector3 point, int layer) {
        return Physics.CheckBox(point, this.transform.lossyScale / 2, this.transform.rotation, layer);
    }
}
