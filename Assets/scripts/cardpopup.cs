using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class CardPopup : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;

    public event Action onCardClosed;
    private bool isVisible = false;
    private CardData currentCard;

    void Start()
    {
        gameObject.SetActive(true);
    }

    public void ShowCard(CardData card)
    {
        if (card == null) return;

        currentCard = card;

        gameObject.SetActive(true);
        isVisible = true;

        cardImage.sprite = card.artwork;
        cardNameText.text = card.cardName;
        descriptionText.text = card.description;

        // Trigger card effect after showing
        StartCoroutine(ActivateEffectAfterPopup());
    }

    IEnumerator ActivateEffectAfterPopup()
    {
        yield return new WaitForSeconds(0.3f);

        if (CardEffectManager.Instance != null && currentCard != null)
        {
            CardEffectManager.Instance.ActivateEffect(currentCard);
        }
    }

    void Update()
    {
        if (isVisible && Input.GetMouseButtonDown(0))
        {
            HideCard();
        }
    }

    public void HideCard()
    {
        gameObject.SetActive(false);
        isVisible = false;
        onCardClosed?.Invoke();
    }
}
