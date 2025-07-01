using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName itemName;

    public void ItemClicked()
    {
        //点击后加入背包
        InventoryManager.Instance.AddItem(itemName);
        //此物品消失
        this.gameObject.SetActive(false);

    }

}
