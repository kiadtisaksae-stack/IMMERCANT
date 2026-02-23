using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EncounterManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string battleSceneName = "BattleScene";
    public GameObject mapRoot; // ลาก GameObject ตัวแม่ที่คุมทั้งฉาก Scene 1 มาใส่ที่นี่

    [Header("Encounter Settings")]
    public float encounterChance = 20f;
    private Camera mapCamera;

    private void Start()
    {
        // เก็บการอ้างอิงกล้องแผนที่ไว้ตั้งแต่เริ่ม
        mapCamera = Camera.main;

        if (mapRoot == null)
        {
            Debug.LogWarning("[EncounterManager] อย่าลืมลาก Map_Root มาใส่นะครับ!");
        }
    }

    public bool WillEncounterTrigger(Waypoint nextPoint)
    {
        // คำนวณโอกาสเกิดเหตุการณ์ตามระดับความอันตราย
        return Random.Range(0f, 100f) <= (encounterChance * nextPoint.dangerLevel);
    }

    public IEnumerator StartBattleEvent(float danger)
    {
        Debug.Log("<color=red>[System] Deactivating Map Root and Switching Scene...</color>");

        // 1. พักการทำงานของฉากแผนที่ (ข้อมูล HP, ไอเทม, ตำแหน่ง ยังอยู่ครบใน RAM)
        if (mapRoot != null) mapRoot.SetActive(false);
        if (mapCamera != null) mapCamera.gameObject.SetActive(false);

        BattleBridge.IsBattleActive = true;
        BattleBridge.EnemyDifficulty = danger;

        // 2. โหลดฉากสู้แบบ Additive (มาวางทับ)
        yield return SceneManager.LoadSceneAsync(battleSceneName, LoadSceneMode.Additive);

        // 3. รอจนกว่าจะสู้จบ (เช็กจากตัวแปรใน Bridge)
        while (BattleBridge.IsBattleActive)
        {
            yield return null;
        }

        // 4. ลบฉากสบู่ออก
        yield return SceneManager.UnloadSceneAsync(battleSceneName);

        // 5. ปลุกฉากแผนที่ให้กลับมาทำงานต่อ
        if (mapRoot != null) mapRoot.SetActive(true);
        if (mapCamera != null) mapCamera.gameObject.SetActive(true);

        Debug.Log("<color=green>[System] Map Root Restored and Camera Re-enabled.</color>");
    }
}