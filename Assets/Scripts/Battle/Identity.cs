using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Identity : MonoBehaviour
{
    [Header("Base Stats")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Base Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 0.3f;
    public int shakeVibrato = 15;
    public float shakeRandomness = 90f;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        // ตั้งค่าฟิสิกส์พื้นฐานที่ทุกตัวต้องมี
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true; // ให้ชนได้แม้จะเป็น Kinematic
        }

        health = maxHealth; // เริ่มต้นเลือดเต็ม
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        // เอฟเฟกต์การสั่นพื้นฐาน
        transform.DOKill();
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {health}");

        if (health <= 0) Die();
    }

    // ใช้ abstract เพื่อบังคับให้ลูกๆ ต้องไปเขียนวิธีตายของตัวเอง
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
