using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;
    private List<ItemSO> cityItems = new List<ItemSO>(); // รายการไอเทมของเมืองปัจจุบัน

    [Header("Slots")]
    public List<itemForbuySlot> buySlots; // ลากใส่จาก Inspector หรือหาอัตโนมัติ
    public List<ItemForSellSlot> sellSlots;

    [Header("Settings")]
    public Transform buyPanel;
    public ItemSO Empty;

    private void Awake() => instance = this;

    // ฟังก์ชันรับข้อมูลไอเทมจากเมือง
    public void InputDataCity(List<ItemSO> inputitem)
    {
        cityItems.Clear(); // ล้างข้อมูลเก่าก่อน
        cityItems.AddRange(inputitem);

        // เมื่อได้รับข้อมูลเมืองใหม่ ให้สั่งเปลี่ยนตลาดทันที
        ChangeMarket(cityItems);
    }

    public void ChangeMarket(List<ItemSO> inputitem)
    {
        // 1. ตรวจสอบว่ามี Slot ซื้อขายในลิสต์หรือยัง ถ้าไม่มีให้หาใน buyPanel
        if (buySlots == null || buySlots.Count == 0)
        {
            buySlots = new List<itemForbuySlot>(buyPanel.GetComponentsInChildren<itemForbuySlot>());
        }

        // 2. ล้างไอเทมเดิมใน Slot ทั้งหมดให้เป็น Empty ก่อน
        foreach (var slot in buySlots)
        {
            slot.SetSlot(Empty);
        }

        // 3. ใส่ไอเทมใหม่จาก input เข้าไปแทนที่ทีละช่อง
        for (int i = 0; i < inputitem.Count; i++)
        {
            // ป้องกัน Error กรณีไอเทมจาก input เยอะกว่าจำนวนช่อง Slot ที่มี
            if (i < buySlots.Count)
            {
                buySlots[i].SetSlot(inputitem[i]);
            }
            else
            {
                Debug.LogWarning("จำนวนไอเทมจากเมืองมีมากกว่าจำนวนช่อง Slot ที่เตรียมไว้!");
                break;
            }
        }
    }
}