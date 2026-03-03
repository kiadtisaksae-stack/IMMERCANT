using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop", menuName = "NPC/Shop List")]
public class ShopListSO : ScriptableObject
{
    public List<ShopItem> itemsForSale;
    public int shopID;

    [System.Serializable]
    public struct ShopItem
    {
        public ItemSO item;
        public int price;
        public int stock; 
        
    }
}