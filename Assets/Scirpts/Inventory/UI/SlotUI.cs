using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Image itemImage;

    public ItemTooltip itemTooltip;

    private ItemDetails currentItem;

    //指的是物品栏被选中
    private bool isSelected;

    public void SetItem(ItemDetails itemDetails)
    {
        currentItem = itemDetails;

        this.gameObject.SetActive(true);

        itemImage.sprite = itemDetails.itemSprite;

        itemImage.SetNativeSize();

    }

    public void SetEmpty()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltip.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.gameObject.activeInHierarchy)
        {
            itemTooltip.gameObject.SetActive(true);
            itemTooltip.UpdateItemName(currentItem.itemName);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        EventHandle.CallItemSelectedEvent(currentItem, isSelected);
    }


}
