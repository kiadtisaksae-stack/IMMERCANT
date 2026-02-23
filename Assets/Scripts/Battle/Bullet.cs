using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 15f;
    public float damage = 20f;

    void Awake()
    {
        // ตั้งค่า Rigidbody2D ให้อัตโนมัติเพื่อให้ชนได้โดยไม่โดนแรงโน้มถ่วงดีด
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // ป้องกันกระสุนทะลุศัตรู

        // ตั้งค่า Collider ให้เป็น Trigger
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                HitTarget(enemy);
            }
        }
    }

    void HitTarget(Enemy enemy)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}