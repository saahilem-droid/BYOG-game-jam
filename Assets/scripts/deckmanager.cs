using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<CardData> allCards = new List<CardData>();  // All cards in deck
    public CardPopup cardPopup;                             // Reference to popup UI
    public CardHandManager cardHandManager;                 // Reference to hand manager
    public DebuffManager debuffManager;                     // Reference to debuff manager
    public GameTimer gameTimer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if player clicked on the deck object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    DrawRandomCard();
                }
            }
        }
    }

void DrawRandomCard()
{
    if (allCards.Count == 0) return;

    int randomIndex = Random.Range(0, allCards.Count);
    CardData drawnCard = allCards[randomIndex];
    allCards.RemoveAt(randomIndex);

    if (cardPopup != null)
        cardPopup.ShowCard(drawnCard);

    switch (drawnCard.effectType)
    {
        case CardEffect.BlockNextDebuff:
            if (cardHandManager != null)
                cardHandManager.AddCardToHand(drawnCard);
            break;

        case CardEffect.ReducePoints:
        case CardEffect.ReduceTime:
            DebuffManager.Instance.TryApplyDebuff(drawnCard);
            break;

        default:
            ApplyInstantEffect(drawnCard); // normal instant effects
            break;
    }
}

    void ApplyDebuffImmediately(CardData card)
    {
        switch (card.effectType)
        {
            /*case CardEffect.ReducePoints:
                ScoreManager.Instance.ReducePoints(Mathf.RoundToInt(card.effectValue));
                break;*/

            case CardEffect.ReduceTime:
                if (GameTimer.Instance != null)
                    GameTimer.Instance.ReduceTime(card.effectValue);
                break;
        }
        Debug.Log(" Debuff applied immediately: {card.cardName}");
    }

    void ApplyInstantEffect(CardData card)
    {
        switch (card.effectType)
        {
            /*case CardEffect.AddPoints:
                ScoreManager.Instance.AddPoints(Mathf.RoundToInt(card.effectValue));
                break;*/

            case CardEffect.AddTime:
                if (GameTimer.Instance != null)
                    GameTimer.Instance.AddTime(card.effectValue);
                break;
        }
        Debug.Log("Effect applied immediately: {card.cardName}");
    }
}
