using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CartSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Slot Data")]
    public ItemSO item;       // เก็บข้อมูลไอเทม
    public int stack;         // จำนวนไอเทมในตะกร้า

    [Header("UI References")]
    public Image iconImage;           // รูปไอคอนไอเทม
    public TextMeshProUGUI amountText; // ตัวเลขแสดงจำนวน

    public void OnPointerClick(PointerEventData eventData)
    {
        // บัคแก้: ถ้าเป็นช่องว่าง ไม่ต้องทำอะไร
        if (item == null || item == MarketManager.instance.Empty) return;

        // Logic: คลิกที่ไอเทมในตะกร้าเพื่อ "ลดจำนวนลง" หรือ "เอาออก"
        RemoveOne();
    }

    private void RemoveOne()
    {
        stack--;

        if (stack <= 0)
        {
            // ถ้าเหลือ 0 ให้ล้างช่องเป็น Empty
            SetSlot(MarketManager.instance.Empty, 0);
        }
        else
        {
            UpdateUI();
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }

    public void SetSlot(ItemSO newItem, int amount)
    {
        item = newItem;
        stack = amount;

        if (iconImage != null)
        {
            // เช็ค Null ป้องกัน Error
            if (item != null && item.icon != null && item != MarketManager.instance.Empty)
            {
                iconImage.sprite = item.icon;
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.color = Color.clear; // โปร่งใสถ้าไม่มีไอเทม
            }
        }
        UpdateUI();
    }

    public void AddAmount(int value)
    {
        stack += value;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (amountText != null)
        {
            // แสดงเลข x2, x3... ถ้ามีแค่ 1 ไม่ต้องแสดง หรือแสดงเป็นช่องว่าง
            amountText.text = (stack > 1) ? "x" + stack.ToString() : "";
        }
    }
}