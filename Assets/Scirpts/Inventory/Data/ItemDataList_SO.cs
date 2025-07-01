using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO",menuName = "Inventory/ItemDataList_SO")]

public class ItemDataList_SO : ScriptableObject
{
    public List<ItemDetails> itemDetailList;

    public ItemDetails GetItemDetails(ItemName itemName)
    {
        return itemDetailList.Find(i => i.itemName == itemName);
    }
}

//从类为物品细节的父类,包含物品相关的详细数据
[System.Serializable]
public class ItemDetails
{
    public ItemName itemName;

    public Sprite itemSprite;

}