using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private IMMInput inputReader;
    [SerializeField] private LayerMask interactableLayer;
    private Camera mainCamera;
    [SerializeField] private MapTravel mapTravel;

    [Header("Movement Settings")]
    public Waypoint currentWaypoint;
    public float moveSpeed = 5f;
    public bool isMoving = false;
    public bool IsMoving { get; private set; }

    private void Awake() => mainCamera = Camera.main;

    private void Start()
    {
        isMoving = false;
        mapTravel = FindAnyObjectByType<MapTravel>();
        if (currentWaypoint != null)
            transform.position = currentWaypoint.transform.position;
    }

    private void OnEnable()
    {
        if (inputReader != null) inputReader.OnMouseClick += HandleClick;

        // รีเซ็ตสถานะการเดินทุกครั้งที่แมพเปิดขึ้นมาใหม่ ป้องกันอาการค้าง
        isMoving = false;
        IsMoving = false;
    }
    private void OnDisable() { if (inputReader != null) inputReader.OnMouseClick -= HandleClick; }

    private void HandleClick(Vector2 mousePosition)
    {
        if (isMoving) return;

        // แก้ไขค่า Z เพื่อให้ Raycast ใน 2D แม่นยำขึ้น
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(mainCamera.transform.position.z)));
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, interactableLayer);

        if (hit.collider != null && hit.collider.TryGetComponent<Castle>(out Castle castle))
        {
            castle.Interact();
        }
    }

    public void SetDestination(Waypoint target)
    {
        if (isMoving || currentWaypoint == target) return;

        WaypointManager manager = FindObjectOfType<WaypointManager>();
        if (manager == null) return;

        // เรียกใช้ A* ที่เราเขียนใหม่
        List<Waypoint> path = manager.FindShortestPath(currentWaypoint, target);

        if (path != null && path.Count > 1)
        {
            path.RemoveAt(0); // ตัดจุดที่ยืนอยู่ปัจจุบันออก
            StartCoroutine(FollowPath(path));
        }
    }

    private IEnumerator FollowPath(List<Waypoint> path)
    {
        isMoving = true;
        IsMoving = true;
        foreach (var nextPoint in path)
        {
            // 1. เช็กดวงก่อนเลยว่า "ระหว่างทางไปจุดหน้า" จะโดนไหม?
            if (mapTravel.CheckForEncounter(nextPoint.dangerLevel))
            {
                // เดินไปหยุดกลางทาง
                Vector3 midPoint = Vector3.Lerp(transform.position, nextPoint.transform.position, 0.5f);
                yield return transform.DOMove(midPoint, (1f / moveSpeed) * 0.5f).WaitForCompletion();

                // สั่งเริ่มฉากสู้และรอจนกว่าจะจบ
                yield return StartCoroutine(mapTravel.StartBattleProcess(nextPoint.dangerLevel));

                // หลังจากสู้จบ (MapTravel เปิดแมพคืนแล้ว) ให้เดินต่อส่วนที่เหลือ
                yield return transform.DOMove(nextPoint.transform.position, (1f / moveSpeed) * 0.5f).WaitForCompletion();
            }
            else
            {
                // ปลอดภัย: วิ่งยาวไปที่จุดหมาย
                yield return transform.DOMove(nextPoint.transform.position, 1f / moveSpeed)
                    .SetEase(Ease.Linear)
                    .WaitForCompletion();
            }
            currentWaypoint = nextPoint;

        }

        isMoving = false;
        IsMoving = false;
    }


    // ระบบ BFS ที่สถาปัตยกรรมดีขึ้น ไม่กิน RAM
    private List<Waypoint> FindPath(Waypoint start, Waypoint end)
    {
        Queue<Waypoint> queue = new Queue<Waypoint>();
        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            Waypoint current = queue.Dequeue();

            if (current == end)
            {
                // ย้อนรอยกลับไปสร้าง List เส้นทาง
                List<Waypoint> path = new List<Waypoint>();
                while (current != null)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }

            foreach (var neighbor in current.neighbors)
            {
                if (neighbor != null && !cameFrom.ContainsKey(neighbor))
                {
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }
        return null; // หาทางไม่เจอ
    }
}