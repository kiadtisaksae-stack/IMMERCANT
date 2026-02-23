using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : Identity
{
    [Header("Movement")]
    public float speed = 2f;
    public float damage = 10f;

    [Header("Detection")]
    public float detectionInterval = 0.5f; // ค้นหาเป้าหมายใหม่ทุกๆ 0.5 วินาที (ประหยัด CPU)

    private Transform target;
    private float detectionTimer;

    protected override void Awake()
    {
        base.Awake(); // เรียกใช้การตั้งค่า RB/Collider จากคลาสแม่ (Identity)
    }

    void Update()
    {
        // 1. ระบบค้นหาเป้าหมายที่ใกล้ที่สุดแบบเป็นจังหวะ
        detectionTimer += Time.deltaTime;
        if (detectionTimer >= detectionInterval || target == null)
        {
            FindNearestTarget();
            detectionTimer = 0f;
        }

        // 2. เคลื่อนที่เข้าหาเป้าหมาย
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void FindNearestTarget()
    {
        // หา Entity ทั้งหมดในฉากที่ใช้คลาส Identity
        Identity[] allEntities = FindObjectsByType<Identity>(FindObjectsSortMode.None);

        float minDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (Identity entity in allEntities)
        {
            // เงื่อนไข: ห้ามโจมตีพวกเดียวกัน (Enemy) และต้องยังมีชีวิตอยู่
            if (entity is Enemy || entity.health <= 0) continue;

            float dist = Vector2.Distance(transform.position, entity.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = entity.transform;
            }
        }

        target = closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("AD"))
        {
            Debug.Log("collided with " + collision.tag);
            if (collision.TryGetComponent<Identity>(out Identity id))
            {
                id.TakeDamage(damage);
                Die(); // ชนแล้วระเบิดตัวเอง
            }
        }

    }
}