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

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (mapBounds != null) CalculateBounds();
    }
    private void Start()
    {
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

    Vector3 ClampCamera(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float clampedX = Mathf.Clamp(targetPos.x, minX + camWidth, maxX - camWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minY + camHeight, maxY - camHeight);

        return new Vector3(clampedX, clampedY, -10f);
    }
}