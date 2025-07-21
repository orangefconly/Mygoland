using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    private readonly List<Card> drawPile = new List<Card>();
    private readonly List<Card> discardPile = new List<Card>();
    private readonly List<Card> hand = new List<Card>();


    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DisCardAllCardsGA>(DisCardAllCardsPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DisCardAllCardsGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }
    private IEnumerator DrawCardsPerformer (DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amout,drawPile.Count);
        int notDrawAmount = drawCardsGA.Amout - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if (notDrawAmount > 0)
        {
            RefillDeck();
            for (int i = 0;i < notDrawAmount;i++)
            {
                yield return DrawCard();
            }
        }    
    }   
    private IEnumerator DisCardAllCardsPerformer (DisCardAllCardsGA disCardAllCardsGA)
    {
        foreach (var card in hand)
        {
            discardPile.Add(card);
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        DisCardAllCardsGA disCardAllCardsGA = new();
        ActionSystem.Instance.AddRection(disCardAllCardsGA);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddRection(drawCardsGA);
    }
    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        hand.Add (card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card,drawPilePoint.position,drawPilePoint.rotation);
        yield return handView.CardAdd(cardView);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(hand);
        discardPile.Clear();  
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, 0.2f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.2f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
