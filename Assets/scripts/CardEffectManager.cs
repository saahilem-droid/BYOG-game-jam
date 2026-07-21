using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System; // 👈 add this line


public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager Instance;

    private bool debuffBlocked = false; // 🧱 Active block state
    private int shieldCount = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateEffect(CardData card)
    {
        if (card == null)
        {
            Debug.LogWarning("⚠️ Tried to activate a null card!");
            return;
        }

        Debug.Log($"🃏 Activating effect: {card.cardName} ({card.effectType})");

        switch (card.effectType)
        {
            case CardEffect.AddTime:
                AddTime(card.effectValue);
                break;

            case CardEffect.ReduceTime:
                TryApplyDebuff(() => ReduceTime(card.effectValue), "time");
                break;

            case CardEffect.AddPoints:
                AddPoints(Mathf.RoundToInt(card.effectValue));
                break;

            case CardEffect.ReducePoints:
                TryApplyDebuff(() => ReducePoints(Mathf.RoundToInt(card.effectValue)), "points");
                break;

            

            case CardEffect.RandomEvent:
                TriggerRandomEvent();
                break;

            case CardEffect.None:
            default:
                Debug.Log($"No effect assigned for card: {card.cardName}");
                break;

            case CardEffect.RemoveAllShields:
                TryApplyDebuff(RemoveAllShields, "all shields");
            break;

        }
    }

    // 🧱 Handles blocking logic
    /*private void TryApplyDebuff(System.Action debuffAction, string type)
    {
        if (debuffBlocked)
        {
            Debug.Log($"🛡️ Blocked a {type} debuff!");
            debuffBlocked = false; // Consume the shield
        }
        else
        {
            debuffAction.Invoke();
        }
    }*/
  private void RemoveAllShields()
{
    if (shieldCount > 0)
    {
        Debug.Log($"💥 All {shieldCount} shields have been destroyed!");
        shieldCount = 0;
    }
    else
    {
        Debug.Log("⚠️ No shields to remove.");
    }
}



    private void ActivateBlock()
{
    shieldCount++;
    Debug.Log($"🛡️ Shield activated! You now have {shieldCount} shield(s).");
}

private void TryApplyDebuff(Action debuffAction, string debuffName)
{
        if (shieldCount > 0)
        {
            shieldCount--;
            Debug.Log($"🛡️ Blocked a {debuffName} debuff! Remaining shields: {shieldCount}");
            return;
        }
        else

        {
            debuffAction.Invoke();
        }        

}


    private void AddTime(float seconds)
    {
        var timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.AddTime(seconds);
    }

    private void ReduceTime(float seconds)
    {
        var timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.ReduceTime(seconds);
    }

    private void AddPoints(int amount)
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddPoints(amount);
    }


public void ReducePoints(CardData card) // or whatever your signature is
{
    Debug.Log($"Activating effect: {card.cardName} (ReducePoints)  at {Time.time}\n{Environment.StackTrace}");
    // existing code below...
}

    private void ReducePoints(int amount)
    {
       
        
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ReducePoints(amount);
    }

    private void TriggerRandomEvent()
    {
        Debug.Log("🎲 Random event triggered!");
    }
}
