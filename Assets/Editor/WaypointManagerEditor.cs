using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaypointManager manager = (WaypointManager)target;

        GUILayout.Space(15);
        GUILayout.Label("Waypoint Management Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("1. Get All Waypoints From Children", GUILayout.Height(30)))
        {
            Undo.RecordObject(manager, "Update Waypoint List");
            manager.allWaypoints.Clear();
            manager.allWaypoints.AddRange(manager.GetComponentsInChildren<Waypoint>());
            EditorUtility.SetDirty(manager);
            Debug.Log($"<color=green>Success:</color> Found {manager.allWaypoints.Count} waypoints.");
        }

        GUI.backgroundColor = Color.cyan; 
        if (GUILayout.Button("2. Auto-Connect by Distance", GUILayout.Height(40)))
        {
            Undo.RecordObject(manager, "Auto Connect");
            manager.AutoConnectWaypoints();

            foreach (var wp in manager.allWaypoints) EditorUtility.SetDirty(wp);
            EditorUtility.SetDirty(manager);

            Debug.Log("<color=cyan>Success:</color> Waypoints connected based on distance!");
        }
        GUI.backgroundColor = Color.white; 

        if (GUILayout.Button("Clear All Connections"))
        {
            if (EditorUtility.DisplayDialog("Confirm", "Are you sure you want to clear all connections?", "Yes", "No"))
            {
                Undo.RecordObject(manager, "Clear Connections");
                foreach (var wp in manager.allWaypoints)
                {
                    Undo.RecordObject(wp, "Clear WP");
                    wp.neighbors.Clear();
                    EditorUtility.SetDirty(wp);
                }
                Debug.Log("All connections cleared.");
            }
        }
    }
}