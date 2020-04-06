using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Area : MonoBehaviour {

    [HideInInspector]
    [SerializeField]
    public EnumRegionShape areaShape = EnumRegionShape.POINT;
    [HideInInspector]
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private Color color = Color.yellow;

    // Fields for Point

    // Fields for Squares/Cubes
    [HideInInspector]
    [SerializeField]
    [Header("The diameter of the shape on the x axis.")]
    private float xSize = 1;
    [HideInInspector]
    [SerializeField]
    [Header("The diameter of the shape on the z axis.")]
    private float zSize = 1;

    // Used for Cubes.
    [SerializeField]
    [Header("The diameter of the shape on the y axis.")]
    private float ySize = 1;

    // Fields for Circle/Sphere
    [HideInInspector]
    [SerializeField]
    private float radius = 1;

    // Fields for Line
    [HideInInspector]
    [SerializeField]
    private float length = 1;

    private void OnDrawGizmos() {
        Gizmos.color = this.color;
        Vector3 v = this.getPos();
        switch(this.areaShape) {
            case EnumRegionShape.POINT:
                Gizmos.DrawWireSphere(v, 0.1f);
                break;
            case EnumRegionShape.SQUARE:
                Gizmos.DrawWireCube(v, new Vector3(this.xSize, 0, this.zSize));
                break;
            case EnumRegionShape.CIRCLE:
                GizmoHelpers.drawCircle(v, this.radius, 16);
                break;
            case EnumRegionShape.LINE:
                float half = this.length / 2;
                Gizmos.DrawLine(v + this.transform.forward * half, v - this.transform.forward * half);
                break;
            case EnumRegionShape.SPHERE:
                Gizmos.DrawWireSphere(v, this.radius);
                break;
            case EnumRegionShape.CUBE:
                Gizmos.DrawWireCube(v, new Vector3(this.xSize, this.ySize, this.zSize));
                break;
        }
    }

    /// <summary>
    /// Returns a random point within the area.
    /// </summary>
    public Vector3 getRndPoint() {
        Vector3 orgin = this.getPos();
        switch(this.areaShape) {
            case EnumRegionShape.POINT:
                return orgin;
            case EnumRegionShape.SQUARE:
                return orgin + this.transform.rotation * new Vector3(this.rndV(this.xSize), 0, this.rndV(this.zSize));
            case EnumRegionShape.CIRCLE:
                Vector2 v = Random.insideUnitCircle;
                return orgin + new Vector3(v.x, 0, v.y) * this.radius;
            case EnumRegionShape.LINE:
                Vector3 lineStart = this.transform.position - this.transform.forward * (this.length / 2);
                return lineStart + this.transform.forward * Random.Range(0, length);
            case EnumRegionShape.SPHERE:
                return orgin + Random.insideUnitSphere * this.radius;
            case EnumRegionShape.CUBE:
                return orgin + this.transform.rotation * new Vector3(this.rndV(this.xSize), this.rndV(this.ySize), this.rndV(this.zSize));
        }
        return this.transform.position;
    }

    private float rndV(float s) {
        return Random.Range(0, s) - (s / 2);
    }

    /// <summary>
    /// Returns the orgin of this area.
    /// </summary>
    public Vector3 getPos() {
        return this.transform.position + this.offset;
    }

    /// <summary>
    /// Returns the size of the area.
    /// </summary>
    public float getSize() {
        switch(this.areaShape) {
            case EnumRegionShape.POINT:
                return 1;
            case EnumRegionShape.SQUARE:
                return this.xSize * this.zSize;
            case EnumRegionShape.CIRCLE:
                return Mathf.PI * this.radius * this.radius;
            case EnumRegionShape.LINE:
                return 0;  // Lines have no area?
            case EnumRegionShape.SPHERE:
                return this.radius;
            case EnumRegionShape.CUBE:
                return this.xSize * this.ySize * this.zSize;
        }
        return 0;
    }

    /// <summary>
    /// Returns the color to draw the area with in the Editor.
    /// Child classes can override this to preovide specific colors based on the use of the area.
    /// </summary>
    public virtual Color getColor() {
        return Colors.magenta;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Area))]
    public class EditorArea : Editor {

        protected SerializedProperty areaType;
        protected SerializedProperty offset;
        protected SerializedProperty xSize;
        protected SerializedProperty ySize;
        protected SerializedProperty zSize;
        protected SerializedProperty radius;
        protected SerializedProperty length;
        protected SerializedProperty color;

        protected virtual void OnEnable() {
            this.areaType = this.serializedObject.FindProperty("areaShape");
            this.offset = this.serializedObject.FindProperty("offset");
            this.xSize = this.serializedObject.FindProperty("xSize");
            this.ySize = this.serializedObject.FindProperty("ySize");
            this.zSize = this.serializedObject.FindProperty("zSize");
            this.radius = this.serializedObject.FindProperty("radius");
            this.length = this.serializedObject.FindProperty("length");
            this.color = this.serializedObject.FindProperty("color");
        }

        public override void OnInspectorGUI() {
            this.serializedObject.UpdateIfRequiredOrScript();

            Area spawnArea = (Area)this.target;

            EditorGUILayout.PropertyField(this.areaType);
            EditorGUILayout.PropertyField(this.offset);

            switch(spawnArea.areaShape) {
                case EnumRegionShape.POINT:
                    break;
                case EnumRegionShape.SQUARE:
                case EnumRegionShape.CUBE:
                    EditorGUILayout.PropertyField(this.xSize);
                    if(spawnArea.areaShape == EnumRegionShape.CUBE) {
                        EditorGUILayout.PropertyField(this.ySize);
                    }
                    EditorGUILayout.PropertyField(this.zSize);
                    break;
                case EnumRegionShape.CIRCLE | EnumRegionShape.SPHERE:
                    EditorGUILayout.PropertyField(this.radius);
                    break;
                case EnumRegionShape.LINE:
                    EditorGUILayout.PropertyField(this.length);
                    break;
            }

            EditorGUILayout.PropertyField(this.color);

            if(GUILayout.Button("Fix Y")) {
                RaycastHit hit;
                if(Physics.Raycast(spawnArea.transform.position, Vector3.down, out hit, 10)) {
                    spawnArea.transform.position = spawnArea.transform.position.setY(hit.point.y + 0.01f);
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
