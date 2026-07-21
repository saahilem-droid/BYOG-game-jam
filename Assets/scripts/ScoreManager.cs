using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        /* else
        {
            Destroy(gameObject);
        } */
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset score when restarting the game
        ResetScore();

        // Try to find a new score text in the new scene
        if (scoreText == null)
        {
            scoreText = FindObjectOfType<TextMeshProUGUI>(true);
        }

        UpdateScoreUI();
    }

    // ✅ Use this for adding score from other scripts (DebuffManager, etc.)
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        Debug.Log($"🏆 Score +{amount}! Total: {score}");
    }

    // ✅ Also compatible method name for existing calls
    public void AddPoints(int amount)
    {
        AddScore(amount);
    }

    public void ReducePoints(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
        UpdateScoreUI();
        Debug.Log($"💔 Score -{amount}! Total: {score}");
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Dev Feats: {score}";
        else
            Debug.LogWarning("⚠ ScoreManager: scoreText not assigned in Inspector!");
    }
}
