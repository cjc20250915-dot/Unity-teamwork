using UnityEngine;

[ExecuteAlways]
public class PathGizmo : MonoBehaviour
{
    public Color lineColor = Color.cyan;
    public float sphereSize = 0.3f;

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Transform t = transform;
        int childCount = t.childCount;
        if (childCount == 0) return;

        Vector3 prev = t.GetChild(0).position;
        for (int i = 0; i < childCount; i++)
        {
            Transform wp = t.GetChild(i);
            Gizmos.DrawSphere(wp.position, sphereSize);
            if (i > 0)
            {
                Gizmos.DrawLine(prev, wp.position);
                prev = wp.position;
            }
            else
            {
                prev = wp.position;
            }
        }
    }
}
