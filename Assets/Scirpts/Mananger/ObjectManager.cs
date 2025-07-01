using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Dictionary<ItemName,bool> itemAvailableDict = new Dictionary<ItemName,bool>();
    private Dictionary<string,bool> interactiveStateDict = new Dictionary<string,bool>();

    private void OnEnable()
    {
        EventHandle.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandle.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandle.UpdateUIEvent += OnUpDateUIEvent;
    }

    private void OnDisable()
    {

        EventHandle.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandle.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandle.UpdateUIEvent -= OnUpDateUIEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.itemName))
                itemAvailableDict.Add(item.itemName, true);
        }
        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if (interactiveStateDict.ContainsKey(item.name))
                interactiveStateDict[item.name] = item.isDone;
            else    
                interactiveStateDict.Add(item.name, item.isDone);
        }
    }

    private void OnAfterSceneLoadEvent()
    {
        foreach(var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.itemName))
                itemAvailableDict.Add(item.itemName, true);
            else
                item.gameObject.SetActive(itemAvailableDict[item.itemName]);
        }
        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if (interactiveStateDict.ContainsKey(item.name))
                item.isDone = interactiveStateDict[item.name];
            else
                interactiveStateDict.Add(item.name, item.isDone);
        }   
    }

    private void OnUpDateUIEvent(ItemDetails itemDetails, int arg2)
    {
        if (itemDetails != null)
        {
            itemAvailableDict[itemDetails.itemName] = false;
        }
    }
}
