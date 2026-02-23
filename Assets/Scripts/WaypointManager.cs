using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaypointManager : MonoBehaviour
{
    public float connectionDistance = 5f;
    public List<Waypoint> allWaypoints = new List<Waypoint>();

    private void Start()
    {
        // บังคับให้หาจุดทั้งหมดก่อนในตอนเริ่ม
        GetAllWaypoints();
        // รอ 2 วินาทีเพื่อให้ทุกจุดพร้อม แล้วค่อยเชื่อมต่อขากลับอัตโนมัติ
        Invoke("FinalizeConnections", 2f);
    }

    // แก้ Error: 'GetAllWaypoints'
    public List<Waypoint> GetAllWaypoints()
    {
        if (allWaypoints.Count == 0)
        {
            allWaypoints.Clear();
            allWaypoints.AddRange(FindObjectsOfType<Waypoint>());
        }
        return allWaypoints;
    }

    // แก้ Error: 'AutoConnectWaypoints' (สำหรับเรียกจากปุ่ม Tool หรือ Start)
    public void AutoConnectWaypoints()
    {
        allWaypoints.Clear();
        allWaypoints.AddRange(FindObjectsOfType<Waypoint>());

        foreach (var wp in allWaypoints) wp.neighbors.Clear();

        for (int i = 0; i < allWaypoints.Count; i++)
        {
            for (int j = 0; j < allWaypoints.Count; j++)
            {
                if (i == j) continue;
                float dist = Vector2.Distance(allWaypoints[i].transform.position, allWaypoints[j].transform.position);
                if (dist <= connectionDistance)
                {
                    allWaypoints[i].RegisterNeighbor(allWaypoints[j]);
                }
            }
        }
        Debug.Log("Auto-Connected all waypoints!");
    }

    // ระบบเชื่อมขากลับให้เอง: ถ้า A เชื่อม B แล้ว B ต้องเชื่อม A ด้วย
    void FinalizeConnections()
    {
        foreach (var wp in allWaypoints)
        {
            foreach (var neighbor in wp.neighbors.ToList())
            {
                if (neighbor != null) neighbor.RegisterNeighbor(wp);
            }
        }
        Debug.Log("Navigation: Bidirectional paths are ready.");
    }

    // ระบบ A* สำหรับหาเส้นทางที่สั้นที่สุด
    public List<Waypoint> FindShortestPath(Waypoint start, Waypoint end)
    {
        if (start == null || end == null) return null;

        List<Waypoint> openSet = new List<Waypoint> { start };
        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
        Dictionary<Waypoint, float> gScore = new Dictionary<Waypoint, float>();
        Dictionary<Waypoint, float> fScore = new Dictionary<Waypoint, float>();

        foreach (var wp in allWaypoints)
        {
            gScore[wp] = float.MaxValue;
            fScore[wp] = float.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Vector2.Distance(start.transform.position, end.transform.position);

        while (openSet.Count > 0)
        {
            Waypoint current = openSet.OrderBy(wp => fScore[wp]).First();

            if (current == end) return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (var neighbor in current.neighbors)
            {
                if (neighbor == null) continue;
                float tentativeGScore = gScore[current] + Vector2.Distance(current.transform.position, neighbor.transform.position);

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Vector2.Distance(neighbor.transform.position, end.transform.position);

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
        return null;
    }

    private List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint current)
    {
        List<Waypoint> totalPath = new List<Waypoint> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        return totalPath;
    }
}