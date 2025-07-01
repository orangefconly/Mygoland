using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemDataList_SO itemData;

    [SerializeField]private List<ItemName> itemList = new List<ItemName>();

    public void AddItem(ItemName itemName)
    {
        //此项目的物品具有唯一性，保证不会重复获得
        if (!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            EventHandle.CallUpdateUIEvent(itemData.GetItemDetails(itemName), itemList.Count - 1);
        }
    }

    private void OnItemUsedEvent(ItemName itemName)
    {
        itemList.Remove(itemName);
        if (itemList.Count == 0)
        {
            EventHandle.CallUpdateUIEvent(null, -1);
        }
        InventoryUI.Instance.SwitchItem(1);
    }

    private void OnEnable()
    {
        EventHandle.ItemUsedEvent += OnItemUsedEvent;
        EventHandle.ChangeItemEvent += OnChangeItemEvent;
    }

    private void OnDisable()
    {
        EventHandle.ItemUsedEvent -= OnItemUsedEvent;
        EventHandle.ChangeItemEvent -= OnChangeItemEvent;
    }

    public int GetitemListCount()
    { 
        return itemList.Count; 
    }

    private void OnChangeItemEvent(int index)
    {
        ItemDetails itemDetails = itemData.GetItemDetails(itemList[index]);
        EventHandle.CallUpdateUIEvent(itemDetails, index);
    }
}
