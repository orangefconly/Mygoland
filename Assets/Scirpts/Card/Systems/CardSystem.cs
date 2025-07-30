using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    public TMP_Text drawPileAmount;
    public TMP_Text discardPileAmount;

    private readonly List<Card> drawPile = new List<Card>();
    private readonly List<Card> discardPile = new List<Card>();
    private readonly List<Card> hand = new List<Card>();

    private void Update()
    {
        drawPileAmount.text = drawPile.Count.ToString();
        discardPileAmount.text = discardPile.Count.ToString();
    }
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DisCardAllCardsGA>(DisCardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DisCardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
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
        int actualAmount = Mathf.Min(drawCardsGA.Amount,drawPile.Count);
        int notDrawAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if(notDrawAmount>discardPile.Count)
        {
            RefillDeck();
            for (int i = 0; i < discardPile.Count; i++)
            {
                yield return DrawCard();
            }
            notDrawAmount = 0;
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
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        //这里调用的丢弃卡牌是动画上的
        yield return DiscardCard(cardView);

        SpendManaGA spendManaGA = new(playCardGA.Card.Mana);
        ActionSystem.Instance.AddRection(spendManaGA);

        foreach (var effect in playCardGA.Card.Effects)
        {
            PerformEffectGA performEffectGA = new(effect);
            ActionSystem.Instance.AddRection(performEffectGA);
        }
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
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddRection(refillManaGA);
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
        drawPile.AddRange(discardPile);
        discardPile.Clear();  
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.Card);
        cardView.transform.DOScale(Vector3.zero, 0.2f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.2f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
