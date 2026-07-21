using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardHandManager : MonoBehaviour
{
    [Header("Hand UI")]
    public Transform handPanel;         // Assign HandPanel
    public GameObject cardButtonPrefab; // Assign CardButton prefab

    private List<CardData> heldCards = new List<CardData>();

    public void AddCardToHand(CardData card)
    {
        heldCards.Add(card);

        GameObject newCardUI = Instantiate(cardButtonPrefab, handPanel);
        newCardUI.name = card.cardName;

        TextMeshProUGUI nameText = newCardUI.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null) nameText.text = card.cardName;

        Image artImage = newCardUI.GetComponentInChildren<Image>();
        if (artImage != null && card.artwork != null) artImage.sprite = card.artwork;

        Button btn = newCardUI.GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(() => UseCard(card, newCardUI));
    }

    void UseCard(CardData card, GameObject cardUI)
    {
        Debug.Log($"🛡️ Used card: {card.cardName}");
        DebuffManager.Instance.CancelNextDebuff();

        heldCards.Remove(card);
        Destroy(cardUI);
    }

    public bool HasBlockCard()
    {
        return heldCards.Exists(c => c.effectType == CardEffect.BlockNextDebuff);
    }
}
