using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private IMMInput inputReader;
    [SerializeField] private LayerMask interactableLayer;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (inputReader != null) inputReader.OnMouseClick += HandleClick;
    }

    private void OnDisable()
    {
        if (inputReader != null) inputReader.OnMouseClick -= HandleClick;
    }

    private void HandleClick(Vector2 mousePosition)
    {
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, interactableLayer);

        if (hit.collider != null)
        {
            Debug.Log($"[PlayerController] Clicked on: {hit.collider.gameObject.name}");
            // ตรวจสอบว่าสิ่งที่คลิกคือ Castle หรือไม่
            if (hit.collider.TryGetComponent<Castle>(out Castle castle))
            {
                castle.Interact(); // สั่งให้ปราสาทตอบสนอง
            }
        }
        else
        {
            Debug.Log("[PlayerController] Clicked on empty space.");
        }
    }
}