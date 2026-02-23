using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapCameraControl : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 10f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public Button zoomInButton;
    public Button zoomOutButton;
    public Button moveLeft;
    public Button moveRight;
    public Button moveUp;
    public Button moveDown;

    [Header("Boundary")]
    public SpriteRenderer mapBounds;
    private float minX, maxX, minY, maxY;
    private Camera cam;

    [Header("Player Tracking")]
    [SerializeField] private PlayerController player;
    [SerializeField] private float smoothSpeed = 5f;
    [Range(0f, 0.5f)] public float viewportMargin = 0.25f;
    private bool isFocusing = false;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (mapBounds != null) CalculateBounds();
        player = FindAnyObjectByType<PlayerController>();
    }
    private void Start()
    {
        FocusOnPlayer();
        // เพิ่ม Event ให้กับปุ่ม
        if (zoomInButton != null)
            zoomInButton.onClick.AddListener(ZoomIn);
        if (zoomOutButton != null)
            zoomOutButton.onClick.AddListener(ZoomOut);
        if (moveLeft != null)
            moveLeft.onClick.AddListener(MoveLeft);
        if (moveRight != null)
            moveRight.onClick.AddListener(MoveRight);
        if (moveUp != null)
            moveUp.onClick.AddListener(MoveUp);
        if (moveDown != null)
            moveDown.onClick.AddListener(MoveDown);
    }
    void LateUpdate()
    {
        // ทำงานเฉพาะตอนที่ Player กำลังเดิน (IsMoving)
        if (player != null && player.isMoving)
        {
            HandleEdgeFollowing();
        }

    }
    public void FocusOnPlayer()
    {
        if (player == null) return;

        // 1. ล็อกไม่ให้ LateUpdate ทำงานแทรก
        isFocusing = true;

        // 2. คำนวณตำแหน่งที่ต้องไป
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        Vector3 clampedPos = ClampCamera(targetPos);

        // 3. สั่งเลื่อนด้วย DOTween
        transform.DOKill(); // ล้างงานเก่า
        transform.DOMove(clampedPos, 0.8f) // ปรับความเร็วตามชอบ (0.8 วินาที)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => {
                // 4. เมื่อเลื่อนถึงแล้ว ค่อยปลดล็อกให้ระบบตามขอบจอกลับมาทำงาน
                isFocusing = false;
            });
    }
    Vector3 ClampCamera(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float clampedX = Mathf.Clamp(targetPos.x, minX + camWidth, maxX - camWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minY + camHeight, maxY - camHeight);

        return new Vector3(clampedX, clampedY, -10f);
    }
    void HandleEdgeFollowing()
    {
        // 1. แปลงพิกัดโลกของ Player เป็นพิกัด Viewport (0 ถึง 1)
        Vector3 viewportPos = cam.WorldToViewportPoint(player.transform.position);

        Vector3 targetPos = transform.position;
        bool shouldMove = false;

        // 2. เช็กขอบซ้าย-ขวา
        if (viewportPos.x < viewportMargin)
        {
            targetPos.x -= (viewportMargin - viewportPos.x) * moveSpeed;
            shouldMove = true;
        }
        else if (viewportPos.x > 1f - viewportMargin)
        {
            targetPos.x += (viewportPos.x - (1f - viewportMargin)) * moveSpeed;
            shouldMove = true;
        }

        // 3. เช็กขอบบน-ล่าง
        if (viewportPos.y < viewportMargin)
        {
            targetPos.y -= (viewportMargin - viewportPos.y) * moveSpeed;
            shouldMove = true;
        }
        else if (viewportPos.y > 1f - viewportMargin)
        {
            targetPos.y += (viewportPos.y - (1f - viewportMargin)) * moveSpeed;
            shouldMove = true;
        }

        // 4. ถ้าชนขอบ ให้เลื่อนกล้องไปหาจุดนั้นอย่างนุ่มนวลและ Clamp ไว้ไม่ให้หลุดแผนที่
        if (shouldMove)
        {
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
            transform.position = ClampCamera(smoothedPos);
        }
    }
    void CalculateBounds()
    {
        Bounds b = mapBounds.bounds;
        minX = b.min.x; maxX = b.max.x;
        minY = b.min.y; maxY = b.max.y;
    }

    // --- ฟังก์ชันสำหรับปุ่มเคลื่อนที่ ---
    public void MoveLeft() => MoveCamera(Vector3.left);
    public void MoveRight() => MoveCamera(Vector3.right);
    public void MoveUp() => MoveCamera(Vector3.up);
    public void MoveDown() => MoveCamera(Vector3.down);

    void MoveCamera(Vector3 direction)
    {
        Vector3 targetPos = transform.position + (direction * moveSpeed * Time.deltaTime);
        transform.position = ClampCamera(targetPos);
    }

    // --- ฟังก์ชันสำหรับปุ่มซูม ---
    public void ZoomIn()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoomSpeed, minZoom, maxZoom);
        transform.position = ClampCamera(transform.position); // Clamp อีกรอบเผื่อซูมแล้วหลุดขอบ
    }

    public void ZoomOut()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomSpeed, minZoom, maxZoom);
        transform.position = ClampCamera(transform.position);
    }

    
}