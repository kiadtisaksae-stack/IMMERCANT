using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    public static InventoryCanvas Instance;
    [Header("Inventory")]
    public ItemSO Empty_Item;
    public Transform slotPrefab;
    public Transform InventoryPanel;
    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)]
    public int slotAmount = 16;
    public InventorySlot[] inventorySlot;
    [Header("Mini Canvas")]
    public RectTransform miniCanvas;
    public int carftInt;
    [Header("SaveData System")]
    [SerializeField]
    InventoryData invData;
    [SerializeField] protected InventorySlot rigthClickSlot;

    // Start is called before the first frame update
    void Start()
    {

        gridLayoutGroup = InventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorylot();


    }
    void Update()
    {
        //// ตรวจสอบการคลิกเมาส์ซ้ายเพื่อใช้ไอเท็ม
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    FillAllQuickSlotsWithSwap();
        //}
    }
    public void FillAllQuickSlotsWithSwap()
    {
        // สร้างลิสต์เก็บช่อง Inventory ที่มีไอเท็ม (ไม่รวมช่องว่าง)
        List<InventorySlot> nonEmptyInventorySlots = new List<InventorySlot>();
        foreach (InventorySlot slot in inventorySlot)
        {
            if (slot.item != Empty_Item)
            {
                nonEmptyInventorySlots.Add(slot);
            }
        }


    }

    #region Slot Creation Methods

    public virtual void CreateInventorylot()
    {
        inventorySlot = new InventorySlot[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab, InventoryPanel);// gเสก slot ออกมาใน panel
            InventorySlot inveSlot = slot.GetComponent<InventorySlot>();

            inventorySlot[i] = inveSlot;
            inveSlot.iventory = this;
            inveSlot.SetThisSlot(Empty_Item, 0);
        }
    }
    #endregion

    #region Inventory Methods Add Use Drop ohter

    public void MakeThisToTopLayer(bool toTop)
    {
        transform.GetComponent<Canvas>().sortingOrder = toTop ? 1 : -1;
    }
    public virtual void AddItem(ItemSO item, int amount)
    {

        InventorySlot slot = IsEmptySlotLeft(item);
        if (slot == null)
        {
            return;
        }
    }

    public void DestroyItem()
    {
        
    }

    public void RemoveItem(InventorySlot slot)
    {
        slot.SetThisSlot(Empty_Item, 0);
    }


    private InventorySlot FindFirstNonEmptySlot()
    {

        // ตรวจสอบ Inventory Slots ตามลำดับ
        foreach (InventorySlot slot in inventorySlot)
        {
            if (slot.item != Empty_Item)
            {
                return slot;
            }
        }
        return null;
    }
    public void SortItem(bool Ascending = true)
    {

    }

    public InventorySlot IsEmptySlotLeft(ItemSO itemChecker = null, InventorySlot itemslot = null)
    {
        if (inventorySlot == null) return null; // ป้องกันข้อผิดพลาด

        InventorySlot firstEmptySlot = null;
        foreach (InventorySlot slot in inventorySlot)
        {
            if (slot == itemslot)
                continue;
            if (slot.item == itemChecker && slot.stack < slot.item.maxStack)
            {
                return slot;
            }
            else if (slot.item == Empty_Item && firstEmptySlot == null)
            {
                firstEmptySlot = slot;
            }
        }
        return firstEmptySlot;
    }

    public void SetLayoutControlChiad(bool isControlled)
    {
        gridLayoutGroup.enabled = isControlled;
    }
    #endregion
    #region

    public void SetRightClickSlot(InventorySlot slot)
    {
        rigthClickSlot = slot;
    }

    #endregion
    #region SaveLoad Data

    [Serializable]
    public class FullInventoryData
    {
        public InventoryData mainInventory;
        public InventoryData quickSlots;
        public InventorySlotData handSlot;

        public FullInventoryData(InventoryCanvas inv)
        {
            mainInventory = new InventoryData(inv.inventorySlot, inv.slotAmount);
        }
    }

    [Serializable]
    public class InventoryData
    {
        public InventorySlotData[] slotDatas;

        public InventoryData(InventorySlot[] slots, int amount)
        {
            slotDatas = new InventorySlotData[amount];
            for (int i = 0; i < amount; i++)
            {
                slotDatas[i] = new InventorySlotData(slots[i]);
            }
        }
    }

    [Serializable]
    public class InventorySlotData
    {
        public string itemfileName;
        public int stack;

        public InventorySlotData(InventorySlot slot)
        {
            itemfileName = slot.item != null ? slot.item.name : "Empty_Item";
            stack = slot.stack;
        }
    }

    public string SaveData()
    {
        FullInventoryData fullData = new FullInventoryData(this);
        return JsonUtility.ToJson(fullData);
    }

    public void LoadData(string data)
    {
        FullInventoryData fullData = JsonUtility.FromJson<FullInventoryData>(data);

        // โหลด Inventory หลัก
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            LoadSlotData(inventorySlot[i], fullData.mainInventory.slotDatas[i]);
        }

    }

    private void LoadSlotData(InventorySlot slot, InventorySlotData slotData)
    {
        if (slot == null || slotData == null)
        {
            Debug.LogWarning("Slot or SlotData is null");
            return;
        }

        string loadPath = "ItemSO/" + slotData.itemfileName;
        ItemSO item = Resources.Load<ItemSO>(loadPath);

        if (item == null)
        {
            item = Empty_Item;
            Debug.LogWarning("Item not found: " + loadPath);
        }

        // ตรวจสอบว่า slot ถูกกำหนด iventory แล้ว
        if (slot.iventory == null)
        {
            slot.iventory = this;
        }

        slot.SetThisSlot(item, slotData.stack);
        // string loadPath = "ItemSO/" + slotData.itemfileName;
        // ItemSO item = Resources.Load<ItemSO>(loadPath);

        // if (item == null)
        // {
        //     item = Empty_Item;
        //     Debug.LogWarning("Item not found: " + loadPath);
        // }

        // slot.SetThisSlot(item, slotData.stack);
    }
    #endregion
}