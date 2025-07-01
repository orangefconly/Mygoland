using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    public RectTransform hand;
    //��ȡ�������λ�ã�����������任�����������Ļ������Ӧ��λ��
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;

    private bool holdItem;

    private ItemName currentItem;
    private void Update()
    {
        Collider2D hit = ObjectAtMousePosition();
        canClick = hit != null;

        //�ּ����˵Ļ���Ҫ�������λ��
        if (hand.gameObject.activeInHierarchy)
        {
            hand.position = Input.mousePosition;
        }

        if (canClick && Input.GetMouseButtonDown(0))
        {
            ClickAction(hit.gameObject);
        }
    }

    private void OnEnable()
    {
        EventHandle.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandle.ItemUsedEvent += OnItemUsedEvent;
    }

    private void OnDisable()
    {
        EventHandle.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandle.ItemUsedEvent -= OnItemUsedEvent;
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails,bool arg2)
    {
        holdItem = arg2;
        if (arg2)
        {
            currentItem = itemDetails.itemName;
        }
        hand.gameObject.SetActive(holdItem);
    }

    private void ClickAction(GameObject gameObject)
    {
        switch(gameObject.tag)
        {
            case "Teleport":
                var teleport = gameObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Item":
                var item = gameObject.GetComponent<Item>();
                item?.ItemClicked();
                break;
            case "Interactive":
                var interactive = gameObject.GetComponent<Interactive>();
                if (holdItem)
                    interactive?.CheckItem(currentItem);
                else
                    interactive.EmptyClicked();
                break;

        }
    }

    private void OnItemUsedEvent(ItemName itemName)
    {
        currentItem = ItemName.None;
        holdItem = false;
        hand.gameObject.SetActive(holdItem);
    }

    //���ô�������� ������ײ������
    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }
}
