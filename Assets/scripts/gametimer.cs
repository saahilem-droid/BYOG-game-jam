using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // for retry
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 60f;        // Total time in seconds
    private float timeRemaining;
    private bool timerIsRunning = true;

    [Header("UI")]
    public TextMeshProUGUI timerText;    // Assign your TMP text here
    public GameObject gameOverCanvas;    // 👈 assign your GameOver canvas here (new)

    [Header("Light Settings")]
    public Light sunLight;               // Assign your directional light
    public Gradient lightColor;          // Gradient to change color from morning to night
    public AnimationCurve lightIntensityCurve; // Optional: brightness fade over time

    public Material nightSkyboxMaterial;

    public static GameTimer Instance;

    private bool isGameOver = false;     // 👈 added flag

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeRemaining = totalTime;

        if (timerText == null)
            Debug.LogError("⛔ TimerText not assigned!");
        if (sunLight == null)
            Debug.LogError("⛔ Sun Light not assigned!");

        // If no intensity curve, make a simple one
        if (lightIntensityCurve == null)
        {
            lightIntensityCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.2f);
        }

        // Hide Game Over Canvas initially
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
        StartCoroutine(SwitchSkyboxAfterTime(totalTime-91));
    }

    IEnumerator SwitchSkyboxAfterTime(float delay)
    {
        // 1. Wait for the specified time
        yield return new WaitForSeconds(delay);

        // 2. Change the global skybox material instantly
        RenderSettings.skybox = nightSkyboxMaterial;

        // 3. Critically, update the environment lighting to match the new skybox
        DynamicGI.UpdateEnvironment();

        Debug.Log("Skybox changed instantly to Night HDRI.");
    }

    public void AddTime(float seconds)
    {
        timeRemaining += seconds;
    }

    public void ReduceTime(float seconds)
    {
        timeRemaining -= seconds;
        if (timeRemaining < 0) timeRemaining = 0;
    }

    private void Update()
    {
        if (!timerIsRunning || isGameOver)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // Update timer display
            DisplayTime(timeRemaining);

            // Update light
            UpdateLighting();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Debug.Log("✅ Time's up!");
                TriggerGameOver();
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateLighting()
    {
        if (sunLight == null) return;

        float t = 1 - (timeRemaining / totalTime); // normalized 0→1
        sunLight.color = lightColor.Evaluate(t);
        sunLight.intensity = lightIntensityCurve.Evaluate(t) * 1.5f;
        sunLight.transform.rotation = Quaternion.Euler(Mathf.Lerp(25f, -25f, t), 0f, 0f);
    }

    // 👇 Added: Game Over handling (without breaking anything)
    private void TriggerGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // pause the game
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }

    // 👇 Added: Buttons for retry and quit
    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit pressed!");
        Application.Quit();
    }
}
