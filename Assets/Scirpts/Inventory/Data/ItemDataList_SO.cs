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

//����Ϊ��Ʒϸ�ڵĸ���,������Ʒ��ص���ϸ����
[System.Serializable]
public class ItemDetails
{
    public ItemName itemName;

    public Sprite itemSprite;

}