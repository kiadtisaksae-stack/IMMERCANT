using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(BoxCollider2D))]
public class Castle : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float bounceAmount = 0.5f;
    [SerializeField] private float duration = 0.25f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    // ฟังก์ชันที่จะถูกเรียกจาก PlayerController
    public void Interact()
    {
        // ล้าง Tween เก่า (ถ้ามี) เพื่อไม่ให้บัคเวลาคลิกรัวๆ
        transform.DOKill();
        transform.localScale = originalScale;

        // เล่นเอฟเฟกต์เด้งแบบ Yoyo
        transform.DOPunchScale(new Vector3(bounceAmount, bounceAmount, 0), duration, 5, 1f)
            .OnComplete(() => transform.localScale = originalScale);

        Debug.Log($"[Castle] {gameObject.name} was interacted!");

        // ตรงนี้สามารถใส่ logic เพิ่มเติมได้ เช่น เปิดหน้าต่าง MarketManager
    }
}