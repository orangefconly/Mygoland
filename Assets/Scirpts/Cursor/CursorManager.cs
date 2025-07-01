using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    public RectTransform hand;
    //获取鼠标坐标位置：把鼠标的坐标变换到摄像机（屏幕）所对应的位置
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;

    private bool holdItem;

    private ItemName currentItem;
    private void Update()
    {
        Collider2D hit = ObjectAtMousePosition();
        canClick = hit != null;

        //手激活了的话，要跟随鼠标位置
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

    //利用传入的坐标 返回碰撞体数据
    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }
}
