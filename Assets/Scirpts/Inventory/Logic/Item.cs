using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName itemName;

    public void ItemClicked()
    {
        //�������뱳��
        InventoryManager.Instance.AddItem(itemName);
        //����Ʒ��ʧ
        this.gameObject.SetActive(false);

    }

}
