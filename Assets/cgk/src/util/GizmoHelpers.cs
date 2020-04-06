using UnityEngine;

public static class GizmoHelpers {

    public static void drawCircle(Vector3 center, float radius, int resolution = 16) {
        float x;
        float z;
        float angle = 20f;
        Vector3 lastPoint = center + new Vector3(0, 0, radius);

        for(int i = 0; i < (resolution + 1); i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Vector3 v = center + new Vector3(x, 0, z);
            Gizmos.DrawLine(lastPoint, v);
            lastPoint = v;

            angle += (360f / resolution);
        }
    }
}