using System.Collections.Generic;
using UnityEngine;

public class Card
{   
    private CardData cardData; 
    
    public string Title => cardData.name;

    public Effect ManualTargetEffects =>cardData.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => cardData.OtherEffects;
    public string Description =>cardData.Description;

    public Sprite Image => cardData.Image;

    public int Mana { get;private set; }

    public Card(CardData data)
    {
        cardData = data;
        Mana = data.Mana;
    }    
}
