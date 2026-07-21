using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    public Sprite artwork;
    [TextArea] public string description;

    [Header("Card Behavior")]
    public CardType cardType;      // Bonus / Debuff / Block
    public CardEffect effectType;  // What the card actually does
    public float effectValue;      // Time change or points

    [Header("Audio")]
    public AudioClip drawSound;    // 🔊 Assign a unique sound per card in Inspector

    // 🔸 Call this when the card is drawn
    public void PlayDrawSound(AudioSource source = null)
    {
        if (drawSound == null) return;

        if (source != null)
        {
            // Play through provided AudioSource (e.g., UI Audio Manager)
            source.PlayOneShot(drawSound);
        }
        else
        {
            // Fallback: play at camera position if no source given
            AudioSource.PlayClipAtPoint(drawSound, Camera.main.transform.position);
        }
    }
}

// Card categories
public enum CardType
{
    Bonus,
    Debuff,
    Block
}

// Effect definitions
public enum CardEffect
{
    None,
    AddTime,
    ReduceTime,
    AddPoints,
    ReducePoints,
    BlockNextDebuff,
    RandomEvent,
    RemoveAllShields
}
