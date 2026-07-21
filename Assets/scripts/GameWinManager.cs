using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    [Header("Win Settings")]
    public int scoreToWin = 14;
    public GameObject gameWonCanvas; // assign in inspector

    private ScoreManager scoreManager;

    private void Awake()
    {
        if (gameWonCanvas != null)
            gameWonCanvas.SetActive(false);

        scoreManager = ScoreManager.Instance;
    }

    private void Update()
    {
        if (scoreManager != null && scoreManager.GetScore() >= scoreToWin)
        {
            ShowWinCanvas();
        }
    }

    public void ShowWinCanvas()
    {
        if (gameWonCanvas != null)
            gameWonCanvas.SetActive(true);

        // Optional: pause the game
        Time.timeScale = 0f;
    }

    public void HideWinCanvas()
    {
        if (gameWonCanvas != null)
            gameWonCanvas.SetActive(false);

        Time.timeScale = 1f;
    }
}
