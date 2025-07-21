using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInteraction : Singleton<CardInteraction>
{
    public bool PlayerIsDragging { get; set; } = false;

    public bool PlayerCanInteract()
    {
        if(!ActionSystem.Instance.IsPreforming) return true;
        else return false;
    }

    public bool PlayerCanHover()
    {
        if(PlayerIsDragging) return false;
        return true;
    }
}
    

