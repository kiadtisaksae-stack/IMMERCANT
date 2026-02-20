using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector(); // วาด UI ปกติ

        WaypointManager manager = (WaypointManager)target;

        GUILayout.Space(10);
        if (GUILayout.Button("1. Get All Waypoints in Children", GUILayout.Height(30))) {
            manager.allWaypoints.Clear();
            manager.allWaypoints.AddRange(manager.GetComponentsInChildren<Waypoint>());
        }

        if (GUILayout.Button("2. Auto-Connect by Distance", GUILayout.Height(40))) {
            manager.AutoConnectWaypoints();
        }

        if (GUILayout.Button("Clear All Connections", GUILayout.Height(20))) {
            foreach (var wp in manager.allWaypoints) wp.neighbors.Clear();
        }
    }
}