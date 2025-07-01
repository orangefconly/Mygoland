using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Singleton<InventoryUI>
{
    public Button leftButton, rightButton;

    public int currentIndex;

    public SlotUI slotUI;

    private void OnEnable()
    {
        EventHandle.UpdateUIEvent += OnUpdateUIEvent;
    }
    private void OnDisable()
    {
        EventHandle.UpdateUIEvent -= OnUpdateUIEvent;
    }
    private void OnUpdateUIEvent(ItemDetails itemDetails, int index)
    {
        var listCount = InventoryManager.Instance.GetitemListCount();
        if (itemDetails == null)
        {
            slotUI.SetEmpty();
            currentIndex = -1;
            leftButton.interactable = false;
            rightButton.interactable = false;

        }
        else
        {
            currentIndex = index;
            slotUI.SetItem(itemDetails);
            if (listCount > 1)
            {
                leftButton.interactable = true;
                rightButton.interactable = true;
            }
        }
    }
    public void SwitchItem(int count)
    {
        var listCount = InventoryManager.Instance.GetitemListCount();
        var index = currentIndex + count;
        if (index < 0)
            index = listCount - 1;
        else if (index > listCount - 1)
            index = 0;
        if (listCount > 1)
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
        else
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
        }
        EventHandle.CallChangeItemEvent(index);
    }
}
