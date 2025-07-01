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
                itemNameText.text = "Կ��";
                break;
            case ItemName.Ticket:
                itemNameText.text = "��Ʊ";
                break;
            default:
                itemNameText.text = "";
                break;
        }

    }
}
