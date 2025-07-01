using System;
using UnityEngine;

public static class EventHandle
{   
    //UI更新事件 更新相应物品和其序号
    public static event Action<ItemDetails, int> UpdateUIEvent;

    public static void CallUpdateUIEvent(ItemDetails itemDetails,int index)
    {
        UpdateUIEvent?.Invoke(itemDetails,index);
    }

    //场景更换事件
    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

    //选中物品事件
    public static event Action<ItemDetails, bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails itemDetails ,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails ,isSelected);
    }

    public static event Action<ItemName> ItemUsedEvent;

    public static void CallItemUsedEvent(ItemName itemName)
    {
        ItemUsedEvent?.Invoke(itemName);
    }

    public static event Action<int> ChangeItemEvent;

    public static void CallChangeItemEvent(int index)
    {
        ChangeItemEvent?.Invoke(index);
    }
}
