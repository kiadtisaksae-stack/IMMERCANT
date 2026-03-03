using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public int price; // ราคาพื้นฐาน
    public bool isStackable = true; // ซ้อนกันได้ไหม (เช่น ยา 99 ขวด)
}