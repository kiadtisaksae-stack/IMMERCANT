using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Economy")]
    public int Gold { get; private set; }

    [Header("Inventory")]
    public Dictionary<ItemSO, int> inventory = new Dictionary<ItemSO, int>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Gold = 1000;
    }

    public void AddGold(int amount) { Gold += amount; NotifyUI(); }
    public bool SpendGold(int amount)
    {
        if (Gold >= amount) { Gold -= amount; NotifyUI(); return true; }
        return false;
    }

    public void AddItem(ItemSO item, int amount)
    {
        if (inventory.ContainsKey(item)) inventory[item] += amount;
        else inventory.Add(item, amount);
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
        if (LobbyUI.Instance != null) LobbyUI.Instance.UpdateGoldText(Gold);
    }
}