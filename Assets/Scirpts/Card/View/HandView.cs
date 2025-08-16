using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;

    private readonly List<CardView> cards = new();

    public IEnumerator CardAdd (CardView cardView)
    {
        cards.Add(cardView);

        yield return UpdateCardPosition(0.15f);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView =GetCardView(card);
        if(cardView == null ) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(0.15f));
        return cardView;
    }

    private CardView GetCardView (Card card)
    {
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }    
    public IEnumerator UpdateCardPosition(float duration)
    {
        if (cards.Count == 0)
            yield break;

        float cardSpacing = 1f / 5f;
        if (cards.Count > 5)
            cardSpacing = 1f / cards.Count;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float position = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(position);
            Vector3 forward = spline.EvaluateTangent(position);
            Vector3 up = spline.EvaluateUpVector(position);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }

}
