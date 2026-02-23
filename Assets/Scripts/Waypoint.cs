using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> neighbors = new List<Waypoint>();
    public float dangerLevel = 1f;
    public void RegisterNeighbor(Waypoint target)
    {
        if (target != null && !neighbors.Contains(target))
        {
            neighbors.Add(target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.15f);

        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                // ถ้าเชื่อม 2 ทางเป็นสีเขียว ถ้าทางเดียวเป็นสีแดง (ไว้เช็กบัค)
                Gizmos.color = neighbor.neighbors.Contains(this) ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }
}