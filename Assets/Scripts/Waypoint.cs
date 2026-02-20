using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
    // ลิสต์เพื่อนบ้านที่จะถูก Tool คำนวณให้
    public List<Waypoint> neighbors = new List<Waypoint>();

    // วาดเส้นให้เห็นใน Scene View
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors) {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}