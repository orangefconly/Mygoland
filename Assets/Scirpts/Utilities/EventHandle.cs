using System;
using System.Collections.Generic;
using UnityEngine;
using static DialogueData_SO;

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

    //对话事件
    public static event Action<DialogueData_SO.DialogueLine> StartDialogueEvent;

    public static void CallStartDialogueEvent(DialogueData_SO.DialogueLine dialogueLine)
    {
        StartDialogueEvent?.Invoke(dialogueLine);
    }

    public static event Action<DialogueData_SO.DialogueLine> UpdateDialogueEvent;

    public static void CallUpdateDialogueEvent(DialogueData_SO.DialogueLine dialogueLine)
    {
        UpdateDialogueEvent?.Invoke(dialogueLine);
    }

    public static event Action EndDialogerEvent;

    public static void CallEndDialogerEvent()
    {
        EndDialogerEvent?.Invoke();
    }

    public static event Action<List<DialogueData_SO.DialogueLine>> ChoicesAvailableEvent;

    public static void CallChoicesAvailableEvent(List<DialogueData_SO.DialogueLine> dialogueLines)
    {
        ChoicesAvailableEvent.Invoke(dialogueLines);
    }

    public static event Action<int> ChoiceSelectedEvent;

    public static void CallChoiceSelectedEvent(int choice)
    {
        ChoiceSelectedEvent.Invoke(choice);
    }
}
