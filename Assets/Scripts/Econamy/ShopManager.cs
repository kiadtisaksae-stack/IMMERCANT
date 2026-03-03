using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public enum ShopMode { Buy, Sell }
    public ShopMode currentMode = ShopMode.Buy;

    [Header("Cart Data")]
    private Dictionary<ItemSO, int> cart = new Dictionary<ItemSO, int>();

    [Header("UI References")]
    public Transform shopGrid;
    public GameObject shopSlotPrefab;
    public TextMeshProUGUI totalCostText;

    private void Awake() => Instance = Instance == null ? this : Instance;

    // เรียกใช้จากปุ่ม Mode Buy/Sell
    public void SetShopMode(int modeIndex)
    {
        currentMode = (ShopMode)modeIndex;
        CancelAll(); // ล้างตะกร้าเมื่อเปลี่ยนโหมด
        RefreshShopDisplay();
    }

    public void UpdateCart(ItemSO item, int amount)
    {
        if (amount <= 0) cart.Remove(item);
        else cart[item] = amount;

        UpdateTotalCost();
    }

    private void UpdateTotalCost()
    {
        int total = 0;
        foreach (var entry in cart) total += entry.Key.price * entry.Value;
        totalCostText.text = "Total: " + total.ToString("N0");
    }

    public void ConfirmTransaction()
    {
        int total = 0;
        foreach (var entry in cart) total += entry.Key.price * entry.Value;

        if (currentMode == ShopMode.Buy)
        {
            if (GameManager.Instance.SpendGold(total))
            {
                foreach (var entry in cart) GameManager.Instance.AddItem(entry.Key, entry.Value);
                Debug.Log("Purchase Confirmed!");
            }
        }
        else // Sell Mode
        {
            foreach (var entry in cart)
            {
                // เช็กว่ามีของในกระเป๋าจริงๆ ไหมก่อนขาย
                GameManager.Instance.RemoveItem(entry.Key, entry.Value);
                GameManager.Instance.AddGold(entry.Key.price * entry.Value);
            }
        }
        CancelAll();
    }

    public void CancelAll()
    {
        cart.Clear();
        UpdateTotalCost();
        // สั่งให้ทุกลูก (Slot) รีเซ็ตตัวเลข
        foreach (Transform child in shopGrid)
        {
            child.GetComponent<ShopSlot>().ResetSlot();
        }
    }

    public void RefreshShopDisplay()
    {
        // ตรงนี้คุณสามารถดึง ItemSO ทั้งหมดในเกมมาโชว์ (ถ้าเป็นโหมดซื้อ) 
        // หรือดึงเฉพาะของในตัวมาโชว์ (ถ้าเป็นโหมดขาย)
    }
}