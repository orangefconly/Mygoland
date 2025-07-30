using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData_SO", menuName = "Data/CardData_SO")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description{ get; private set; }
    [field: SerializeField] public int Mana{ get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeReference, SR] public List<Effect> Effects { get; private set; }
}