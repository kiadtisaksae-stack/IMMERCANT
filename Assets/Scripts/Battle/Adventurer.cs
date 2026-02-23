using UnityEngine;
using DG.Tweening;

public enum AdventurerType { Melee, Ranged }

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Adventurer : Identity
{
    public float Damage = 15f;
    public AdventurerType type;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackRange = 3f;
    public float fireRate = 1f;
    public LayerMask enemyLayer;

    private float fireCountdown = 0f;
    private Animator animator;
    private bool isAttack;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    void Update()
    {
        if (fireCountdown <= 0f)
        {
            Attack();
            fireCountdown = 1f / fireRate;
        }

        // ควบคุม Animator ให้เล่น Animation โจมตี
        if (animator != null)
        {
            animator.SetBool("isAttack", isAttack);
        }

        fireCountdown -= Time.deltaTime;
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // ถ้าไม่มีศัตรูในระยะ ให้หยุดโจมตี
        if (hitEnemies.Length == 0)
        {
            isAttack = false;
            return;
        }

        Transform nearest = GetNearest(hitEnemies);

        if (nearest != null)
        {
            isAttack = true;

            // --- เพิ่มระบบหันหน้า ---
            FaceTarget(nearest);

            if (type == AdventurerType.Ranged) Shoot(nearest,Damage);
            else nearest.GetComponent<Identity>().TakeDamage(Damage);
        }
        else
        {
            isAttack = false;
        }
    }

    // ฟังก์ชันคำนวณการหันซ้าย-ขวา
    void FaceTarget(Transform target )
    {
        // ถ้าเป้าหมายอยู่ทางขวาของตัวเรา (X มากกว่า)
        if (target.position.x > transform.position.x)
        {
            // หันไปทางขวา (0 องศา)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // ถ้าเป้าหมายอยู่ทางซ้ายของตัวเรา (X น้อยกว่า)
        else
        {
            // หันไปทางซ้าย (180 องศา)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private Transform GetNearest(Collider2D[] hits)
    {
        Transform best = null;
        float dist = Mathf.Infinity;
        foreach (var h in hits)
        {
            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d < dist) { dist = d; best = h.transform; }
        }
        return best;
    }

    void Shoot(Transform target ,float damage)
    {
        Vector3 pos = firePoint != null ? firePoint.position : transform.position;
        GameObject b = Instantiate(bulletPrefab, pos, transform.rotation); // ใช้ rotation ของตัวละครเพื่อให้กระสุนหันถูกทิศ
        b.GetComponent<Bullet>().damage = damage; 
        b.GetComponent<Bullet>().Seek(target);
    }
}