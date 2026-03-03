using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening; // ต้องใช้ DOTween

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image icon;
    public TextMeshProUGUI countText;

    [Header("Settings")]
    [SerializeField] private float selectScale = 1.15f; // ขนาดตอนถูกเลือก (115%)
    [SerializeField] private float animDuration = 0.2f;

    private ItemSO currentItem;
    private Vector3 originalScale;
    private bool isSelected = false;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void SetItem(ItemSO item, int count)
    {
        currentItem = item;
        icon.sprite = item.icon;
        countText.text = count > 1 ? count.ToString() : "";

        // รีเซ็ตสถานะเมื่อวาดใหม่
        isSelected = false;
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // แจ้ง LobbyUI ให้จัดการเรื่อง "ใครถูกเลือก"
            LobbyUI.Instance.SelectSlot(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // ทำลายไอเทม
            OnDestroyItem();
        }
    }

    // ฟังก์ชันสั่งเด้งและขยายค้างไว้
    public void SelectAnim()
    {
        isSelected = true;
        transform.DOKill(); // หยุด Tween เก่าก่อน
        // เล่นเอฟเฟกต์เด้ง (Punch) แล้วขยายค้างไว้ที่ selectScale
        transform.DOPunchScale(Vector3.one * 0.1f, animDuration, 5, 1f);
        transform.DOScale(originalScale * selectScale, animDuration).SetEase(Ease.OutBack);
    }

    // ฟังก์ชันสั่งหดกลับขนาดเดิม
    public void DeselectAnim()
    {
        isSelected = false;
        transform.DOKill();
        transform.DOScale(originalScale, animDuration).SetEase(Ease.OutQuad);
    }

    private void OnDestroyItem()
    {
        GameManager.Instance.RemoveItem(currentItem, 1);
        LobbyUI.Instance.RefreshInventoryUI();
    }
}