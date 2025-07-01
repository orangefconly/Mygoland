using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;

    public void UpdateItemName(ItemName itemName)
    {
        switch (itemName)
        {
            case ItemName.Key:
                itemNameText.text = "Ô¿³×";
                break;
            case ItemName.Ticket:
                itemNameText.text = "´¬Æ±";
                break;
            default:
                itemNameText.text = "";
                break;
        }

    }
}
