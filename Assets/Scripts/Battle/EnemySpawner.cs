using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public BattleManager battleManager;
    [Header("Spawn Locations")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Enemy Variety")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Spawner Settings")]
    public float spawnRate = 3f;        // ระยะห่างระหว่างแต่ละระลอก (วินาที)
    public int totalAmount = 30;        // จำนวนศัตรูทั้งหมดในด่านนี้

    [Header("Simultaneous Settings")]
    public int minSimultaneous = 1;     // จำนวนเกิดขั้นต่ำต่อรอบ
    public int maxSimultaneous = 3;     // จำนวนเกิดสูงสุดต่อรอบ (เช่น สุ่มเกิดพร้อมกัน 1-3 ตัว)

    private int spawnedCount = 0;

    void Start()
    {
        // ค้นหา BattleManager อัตโนมัติถ้าลืมลากใส่
        //if (battleManager == null) battleManager = FindFirstObjectByType<BattleManager>();

        //if (spawnPoints.Count > 0 && enemyPrefabs.Count > 0)
        //{
        //    StartCoroutine(SpawnRoutine());
        //}
    }
    private void OnEnable()
    {
        // รีเซ็ตสถานะการเกิดทุกครั้งที่เปิดขึ้นมาใหม่ ป้องกันอาการค้าง
        spawnedCount = 0;
        if (battleManager == null) battleManager = FindFirstObjectByType<BattleManager>();

        if (spawnPoints.Count > 0 && enemyPrefabs.Count > 0)
        {
            StartCoroutine(SpawnRoutine());
        }
    }
    

    IEnumerator SpawnRoutine()
    {
        // 1. ช่วงเวลาปล่อยศัตรู
        while (spawnedCount < totalAmount)
        {
            int amountThisTime = Random.Range(minSimultaneous, maxSimultaneous + 1);

            for (int i = 0; i < amountThisTime; i++)
            {
                if (spawnedCount >= totalAmount) break;
                SpawnRandomEnemy();
                spawnedCount++;
            }
            yield return new WaitForSeconds(spawnRate);
        }

        Debug.Log("<color=cyan>[Spawner] ส่งศัตรูออกมาครบแล้ว... กำลังรอจัดการให้หมด!</color>");

        // 2. ช่วงเวลาตรวจสอบว่าศัตรูตายหมดหรือยัง
        bool enemiesStillExist = true;
        while (enemiesStillExist)
        {
            // ค้นหา Object ในฉากที่มีคลาส Enemy (หรือ Identity ที่สืบทอดไปเป็น Enemy)
            Enemy[] remainingEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            if (remainingEnemies.Length == 0)
            {
                enemiesStillExist = false; // ถ้าไม่เหลือแล้วให้หลุดลูป
            }
            else
            {
                // รอ 1 วินาทีก่อนเช็กใหม่ (ประหยัด CPU ไม่ต้องเช็กทุกเฟรม)
                yield return new WaitForSeconds(1f);
            }
        }

        // 3. เมื่อศัตรูหมดเกลี้ยงแล้ว สั่งจบการต่อสู้
        Debug.Log("<color=green>[Spawner] ศัตรูหมดแล้ว! ชนะการต่อสู้!</color>");
        if (battleManager != null)
        {
            Debug.Log("<color=yellow>[Spawner] แจ้ง BattleManager ว่าชนะการต่อสู้!</color>");
            battleManager.WinBattle();
        }
    }

    void SpawnRandomEnemy()
    {
        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Transform selectedPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // ใส่ 'transform' (ตัว Spawner เอง) เป็น Parent 
        // เพื่อการันตีว่ามอนสเตอร์จะอยู่ในฉาก Battle และโดนลบพร้อมกัน
        Instantiate(selectedPrefab, selectedPoint.position, Quaternion.identity, transform);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.position, 0.5f);
        }
    }
}