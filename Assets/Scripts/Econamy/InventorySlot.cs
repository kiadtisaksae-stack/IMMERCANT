using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{

    [Header("inventory Detail")]
    public InventoryCanvas iventory;
    [Header("Slot Detail")]
    public ItemSO item;
    public int stack;

    [Header("UI")]
    public Color emptyColor;
    public Color itemColor;
    public Image icons;
    public TextMeshProUGUI stackText;
    [Header("Drag and Drop")]
    public int siblingIndex;
    public int craftInts;


    protected virtual void Start()
    {
        siblingIndex = transform.GetSiblingIndex();
    }
    void Update()
    {

    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == iventory.Empty_Item || iventory == null)
                return;

        }


    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }



    public virtual void SetThisSlot(ItemSO newItem, int amount)
    {
        item = newItem;
        Debug.Log(icons.name);
        Debug.Log(newItem.icon);
        icons.sprite = newItem.icon;


        int ItemAmount = amount;//เก็บค่า amount ไว้กับ itemAmout

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, newItem.maxStack);// รับค่า itemAmout ว่าเกืน newItem มั้ย ท่าเกินตัดออก
        stack = intInthisSlot;

        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = iventory.IsEmptySlotLeft(newItem, this);//check slot ว่าง
            if (slot == null)
            {
                return;
            }
            else
            {
                slot.SetThisSlot(newItem, amountLeft);
            }
        }
    }


    
}
