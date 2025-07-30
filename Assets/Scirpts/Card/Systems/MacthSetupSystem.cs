using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacthSetupSystem : MonoBehaviour
{
    /*
     * [SerializeField] private List<CardData> deckcards = new List<CardData>();
    */
    [SerializeField] private int initDrawNumber = 5;

    [SerializeField] private List<EnemyData> enemyDatas;
    [SerializeField] private HeroData heroData;
    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        EnemySystem.Instance.Setup(enemyDatas);
        CardSystem.Instance.Setup(heroData.Deck);
        DrawCardsGA drawCardGA = new(initDrawNumber);
        ActionSystem.Instance.Perform(drawCardGA);
    }
}
