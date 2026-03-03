using UnityEngine;
using System.Collections;

public class MapTravel : MonoBehaviour
{
    [Header("Main Roots")]
    public GameObject mapRoot;
    public GameObject battleRoot;

    [Header("Cameras")]
    public GameObject mapCamera;    // ลากกล้องแผนที่มาใส่
    public GameObject battleCamera; // ลากกล้องฉากสู้มาใส่

    [Header("Encounter Settings")]
    public float encounterChance = 20f;

    void Awake()
    {
        // เริ่มต้นด้วยโหมดแผนที่
        ShowMap();
    }
    private void DisableAllCapitalColliders()
    {
        // ค้นหา Capital ทุกตัวในฉาก (รวมถึงตัวที่ซ่อนอยู่)
        Capital[] allCapitals = Object.FindObjectsByType<Capital>(FindObjectsSortMode.None);
        foreach (Capital cap in allCapitals)
        {
            if (cap.Collider != null)
            {
                cap.Collider.enabled = false;
                Debug.Log($"<color=orange>[MapTravel] Force Disabled Collider of: {cap.gameObject.name}</color>");
            }
        }
    }

    public bool CheckForEncounter(float dangerLevel)
    {
        return Random.Range(0f, 100f) <= (encounterChance * dangerLevel);
    }

    public IEnumerator StartBattleProcess(float danger)
    {
        DisableAllCapitalColliders();
        // 1. สลับพื้นที่
        mapRoot.SetActive(false);
        battleRoot.SetActive(true);

        // 2. สลับกล้อง
        if (mapCamera != null) mapCamera.SetActive(false);
        if (battleCamera != null) battleCamera.SetActive(true);

        BattleBridge.IsBattleActive = true;
        BattleBridge.EnemyDifficulty = danger;

        // รอจนกว่าจะสู้จบ
        while (BattleBridge.IsBattleActive)
        {
            yield return null;
        }
    }

    public void FinishBattleProcess()
    {
        ReturnToMap();
        Debug.Log("<color=green>[MapTravel] Victory! Returning to Map.</color>");
    }

    public void LoseBattleProcess()
    {
        ReturnToMap();
        Debug.Log("<color=red>[MapTravel] Defeat! Returning to Map.</color>");
    }

    private void ReturnToMap()
    {
        // 1. สลับพื้นที่กลับ
        battleRoot.SetActive(false);
        mapRoot.SetActive(true);

        // 2. สลับกล้องกลับ
        if (battleCamera != null) battleCamera.SetActive(false);
        if (mapCamera != null) mapCamera.SetActive(true);

        BattleBridge.IsBattleActive = false;
    }

    private void ShowMap()
    {
        if (mapRoot != null) mapRoot.SetActive(true);
        if (battleRoot != null) battleRoot.SetActive(false);

        if (mapCamera != null) mapCamera.SetActive(true);
        if (battleCamera != null) battleCamera.SetActive(false);
    }
}