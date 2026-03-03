using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(BoxCollider2D))]
public class Castle : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float bounceAmount = 0.5f;
    [SerializeField] private float duration = 0.25f;
    [Header("Navigation")]
    public Waypoint entryPoint;
    public string villageName;
    private Capital capital;
    //public List<Village> neighbors;

    private Vector3 originalScale;
    [HideInInspector]
    public Vector2 position;

    private void Start()
    {
        originalScale = transform.localScale;
        if (entryPoint == null)
        {
            FindNearestWaypoint();
        }
        capital = GetComponent<Capital>();
    }

    // ฟังก์ชันที่จะถูกเรียกจาก PlayerController
    public void Interact()
    {
        // ตรวจสอบก่อนว่า capital และ Collider มีตัวตนจริงไหม
        if (capital != null && capital.Collider != null)
        {
            capital.Collider.enabled = true;
        }
        else
        {
            // ถ้ามันพัง จะได้รู้ว่าตัวไหนหายไป
            Debug.LogError($"[Castle] {gameObject.name} หา Capital หรือ Collider ไม่เจอ!");
            return;
        }
        // ล้าง Tween เก่า (ถ้ามี) เพื่อไม่ให้บัคเวลาคลิกรัวๆ
        transform.DOKill();
        transform.localScale = originalScale;

        // เล่นเอฟเฟกต์เด้งแบบ Yoyo
        transform.DOPunchScale(new Vector3(bounceAmount, bounceAmount, 0), duration, 5, 1f)
            .OnComplete(() => transform.localScale = originalScale);

        Debug.Log($"[Castle] {gameObject.name} was interacted!");
        PlayerController navigator = FindFirstObjectByType<PlayerController>();
        if (navigator != null && entryPoint != null)
        {
            navigator.SetDestination(entryPoint);
            MapCameraControl cameraControl = FindFirstObjectByType<MapCameraControl>();
            if (cameraControl != null)
            {
                cameraControl.FocusOnPlayer();
            }
        }

    }
    private void FindNearestWaypoint()
    {
        WaypointManager manager = FindAnyObjectByType<WaypointManager>();

        if (manager == null)
        {
            Debug.LogError($"[Castle] {gameObject.name} cannot find WaypointManager in scene!");
            return;
        }

        // ดึง List ของ Waypoints ทั้งหมดมาเช็ก
        var waypoints = manager.GetAllWaypoints();

        Waypoint closest = null;
        float minDistance = float.MaxValue;
        Vector2 myPos = transform.position;

        foreach (var wp in waypoints)
        {
            float dist = Vector2.Distance(myPos, wp.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = wp;
            }
        }

        if (closest != null)
        {
            entryPoint = closest;
            Debug.Log($"[Castle] {gameObject.name} found and linked to: {closest.name}");
        }
    }
}