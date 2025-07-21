using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacthSetupSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckcards = new List<CardData>();

    [SerializeField] private int initDrawNumber = 5;

    private void Start()
    {
        CardSystem.Instance.Setup(deckcards);
        DrawCardsGA drawCardGA = new(initDrawNumber);
        ActionSystem.Instance.Perform(drawCardGA);
    }
}
