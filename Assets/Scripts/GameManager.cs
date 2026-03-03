using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Economy")]
    public int Gold { get; private set; }

    [Header("Inventory")]
    // เก็บไอเทมเป็น <ข้อมูลไอเทม, จำนวน>
    public Dictionary<ItemSO, int> inventory = new Dictionary<ItemSO, int>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Gold = 500; // เงินเริ่มต้น
    }

    // --- ระบบเงิน ---
    public void AddGold(int amount)
    {
        Gold += amount;
        NotifyUI();
    }

    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            NotifyUI();
            return true;
        }
        return false;
    }

    // --- ระบบไอเทม ---
    public void AddItem(ItemSO item, int amount)
    {
        if (inventory.ContainsKey(item))
            inventory[item] += amount;
        else
            inventory.Add(item, amount);

        Debug.Log($"[Inventory] ได้รับ {item.itemName} x{amount}");
    }

    public void RemoveItem(ItemSO item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= amount;
            if (inventory[item] <= 0) inventory.Remove(item);
        }
    }

    private void NotifyUI()
    {
        // สั่งให้ LobbyUI อัปเดตตัวเลขเงิน
        LobbyUI lobby = FindFirstObjectByType<LobbyUI>();
        if (lobby != null) lobby.UpdateGoldText(Gold);
    }
}