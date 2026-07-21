using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public void AddBonus()
    {
        score += 10;
        UpdateScoreUI();
    }

    public void SubtractDebuff()
    {
        score -= 5;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
}
