using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class itemForbuySlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private CartSlot cartslot;
    public ItemSO item;
    public Image iconImage;
    public TextMeshProUGUI amoutText;

    //private void Start()
    //{
    //    cartslot = FindAnyObjectByType<CartSlot>();
    //}
    // ฟังก์ชันสำหรับตั้งค่าไอเทมในช่องนี้
    public void SetSlot(ItemSO newItem)
    {
        item = newItem;

        // อัปเดตรูปภาพ
        if (iconImage != null)
        {
            if (item != null && item.icon != null)
            {
                iconImage.sprite = item.icon;
                iconImage.color = Color.white; // แสดงสีปกติ
            }
            else
            {
                iconImage.sprite = null;
                iconImage.color = new Color(0, 0, 0, 0); // โปร่งใสถ้าไม่มีไอเทม
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || item.itemName == "Empty") return;
        cartslot = FindAnyObjectByType<CartSlot>();
    }

    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }
}