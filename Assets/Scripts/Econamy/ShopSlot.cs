using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image icon;
    public TextMeshProUGUI selectedCountText; // แสดงจำนวนที่กำลังจะซื้อ/ขาย

    private ItemSO itemData;
    private int selectedAmount = 0;
    private bool isHolding = false;
    private Coroutine holdCoroutine;

    public void Setup(ItemSO item)
    {
        itemData = item;
        icon.sprite = item.icon;
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // คลิก 1 ครั้ง เพิ่ม 1 ชิ้น
        AddAmount(1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdCoroutine = StartCoroutine(HoldIncrementRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        if (holdCoroutine != null) StopCoroutine(holdCoroutine);
    }

    private IEnumerator HoldIncrementRoutine()
    {
        yield return new WaitForSeconds(0.5f); // รอ 0.5 วิถึงจะเริ่มรัว
        while (isHolding)
        {
            AddAmount(1);
            yield return new WaitForSeconds(0.1f); // รัวทุก 0.1 วิ
        }
    }

    private void AddAmount(int val)
    {
        if (selectedAmount + val <= itemData.maxStack)
        {
            selectedAmount += val;
            UpdateUI();
            ShopManager.Instance.UpdateCart(itemData, selectedAmount); // แจ้งตะกร้าสินค้า

            // Tween เด้งเบาๆ เมื่อเพิ่มจำนวน
            transform.DOPunchScale(Vector3.one * 0.05f, 0.1f);
        }
    }

    public void ResetSlot()
    {
        selectedAmount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        selectedCountText.text = selectedAmount > 0 ? selectedAmount.ToString() : "";
    }
}