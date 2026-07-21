using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("UI")]
    public Image artworkImage;     // link the Image component on the prefab
    public Text cardNameText;      // optional, link if present

    [HideInInspector]
    public CardData data;

    private AudioSource uiSource => UIAudioManager.Instance?.audioSource;

    // Initialize called by Deck when instantiating
    public void Initialize(CardData cardData)
    {
        data = cardData;
        ApplyVisuals();

        // default: play sound immediately on initialize.
        // If you want to sync with an animation, call PlayDrawSound from an AnimationEvent instead.
        PlayDrawSound();
    }

    void ApplyVisuals()
    {
        if (data == null) return;
        if (artworkImage != null && data.artwork != null) artworkImage.sprite = data.artwork;
        if (cardNameText != null) cardNameText.text = data.cardName;
    }

    public void PlayDrawSound()
    {
        if (data == null) return;

        if (uiSource != null)
        {
            data.PlayDrawSound(uiSource);
        }
        else
        {
            data.PlayDrawSound(null); // fallback
        }
    }

    // Use this method as an Animation Event in your reveal animation for perfect sync.
    public void OnRevealAnimationEvent()
    {
        PlayDrawSound();
    }
}
