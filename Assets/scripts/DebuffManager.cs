using System.Collections;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager Instance;

    private bool debuffPending = false;
    private CardData currentDebuff;

    [Header("Blocking")]
    [Tooltip("How long the player has to click a block after a debuff is drawn")]
    public float blockWindowSeconds = 1.5f;

    private Coroutine waitCoroutine;
   



    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Called when a debuff card is drawn and we want to attempt to apply it.
    /// If the player has a block card, we wait for the block window. Otherwise apply immediately.
    /// </summary>
    public void TryApplyDebuff(CardData debuff)
    {
        currentDebuff = debuff;

        // If player has a block available, enter pending state and start the wait window
        var handManager = FindObjectOfType<CardHandManager>();
        if (handManager != null && handManager.HasBlockCard())
        {
            debuffPending = true;
            Debug.Log($"⚠ Debuff drawn ({debuff.cardName}). Click block card to cancel within {blockWindowSeconds} seconds or accept.");
            // restart coroutine if already running
            if (waitCoroutine != null) StopCoroutine(waitCoroutine);
            waitCoroutine = StartCoroutine(WaitForBlockWindow());
        }
        else
        {
            // no block available, apply immediately
            ApplyDebuff();
        }
    }

    /// <summary>
    /// Can be called by UI block button or block card logic to cancel the pending debuff.
    /// This will also attempt to consume a block from CardHandManager if possible.
    /// </summary>
    public void CancelNextDebuff()
    {
        if (!debuffPending)
        {
            Debug.Log("CancelNextDebuff called but no debuff is pending.");
            return;
        }

        // Try to consume a block card from CardHandManager (if such a method exists).
        var handManager = FindObjectOfType<CardHandManager>();
        if (handManager != null)
        {
            // Attempt common method names safely via reflection to avoid compile errors:
            var t = handManager.GetType();
            var method = t.GetMethod("UseBlockCard") ?? t.GetMethod("ConsumeBlockCard") ?? t.GetMethod("RemoveBlockCard");
            if (method != null)
            {
                method.Invoke(handManager, null);
            }
            else
            {
                // If no method, log a hint — UI code should remove/consume the block button instead.
                Debug.Log("DebuffManager: CardHandManager has no known Consume/UseBlock method. Ensure your block UI removes the block card when clicked.");
            }
        }

        // cancel pending debuff
        debuffPending = false;
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        Debug.Log($"🛡️ Debuff '{currentDebuff?.cardName}' canceled!");
        currentDebuff = null;
    }

    /// <summary>
    /// Alternate entry point: if your block button calls this directly when clicked.
    /// Keeps API clear.
    /// </summary>
    public void OnBlockButtonClicked()
    {
        CancelNextDebuff();
    }

    /// <summary>
    /// Waits for the configured window. If not cancelled, applies the debuff.
    /// </summary>
    private IEnumerator WaitForBlockWindow()
    {
        float t = 0f;
        while (t < blockWindowSeconds)
        {
            t += Time.deltaTime;
            yield return null;
        }

        waitCoroutine = null;

        if (debuffPending)
        {
            // no block clicked in time -> apply it
            ApplyDebuff();
        }
        // else it was cancelled already
    }

    /// <summary>
    /// Performs the actual debuff effect. Uses GameTimer.Instance and ScoreManager if present.
    /// </summary>
    void ApplyDebuff()
    {
        if (currentDebuff == null)
        {
            Debug.LogWarning("ApplyDebuff called but currentDebuff is null.");
            debuffPending = false;
            return;
        }

        Debug.Log($"💀 Debuff applied: {currentDebuff.cardName}");

        // Example effect handling — extend as needed
        switch (currentDebuff.effectType)
        {
            case CardEffect.ReduceTime:
                if (GameTimer.Instance != null)
                {
                    GameTimer.Instance.ReduceTime(currentDebuff.effectValue);
                }
                else
                {
                    Debug.LogWarning("GameTimer.Instance not found. Can't reduce time.");
                }
                break;

            case CardEffect.ReducePoints:
                // Try ScoreManager if you have one
                var scoreMgrType = typeof(object);
                var scoreMgr = FindObjectOfType<MonoBehaviour>(); // fallback find to check for ScoreManager by name
                // safer check by name:
                var sm = FindObjectOfType<ScoreManager>();
                if (sm != null)
                {
                    sm.AddScore(-Mathf.RoundToInt(currentDebuff.effectValue));
                }
                else
                {
                    Debug.LogWarning("ScoreManager not found. Can't reduce points. Implement ScoreManager or handle this effect.");
                }
                break;

            // If you want other effect types handled here, add them:
            case CardEffect.AddTime:
                if (GameTimer.Instance != null)
                {
                    GameTimer.Instance.AddTime(currentDebuff.effectValue);
                }
                else
                {
                    Debug.LogWarning("GameTimer.Instance not found. Can't add time.");
                }
                break;

            case CardEffect.AddPoints:
                var sm2 = FindObjectOfType<ScoreManager>();
                if (sm2 != null)
                {
                    sm2.AddScore(Mathf.RoundToInt(currentDebuff.effectValue));
                }
                else
                {
                    Debug.LogWarning("ScoreManager not found. Can't add points.");
                }
                break;

            default:
                Debug.Log($"Debuff effect '{currentDebuff.effectType}' not implemented in DebuffManager.ApplyDebuff().");
                break;
        }

        // clear pending state and current debuff
        debuffPending = false;
        currentDebuff = null;
    }
}
