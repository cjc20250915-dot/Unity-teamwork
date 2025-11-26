#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CreateWaypointTool
{
    [MenuItem("GameObject/UrbanFlow/Create Waypoint", false, 0)]
    public static void CreateWaypoint()
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogWarning("Please select a parent object (e.g., Road_1) to create a waypoint under.");
            return;
        }
        GameObject go = new GameObject("WP_New");
        go.transform.SetParent(Selection.activeTransform, false);
        // position at parent's position for convenience
        go.transform.localPosition = Vector3.zero;
        // set a handleable default
        Selection.activeGameObject = go;
    }
}
#endif
