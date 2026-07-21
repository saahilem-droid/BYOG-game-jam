using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        // Check for ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Called when Pause button on screen is pressed
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;  // Freeze time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;  // Resume time
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        // This stops Play Mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // This closes the app in the built version
        Application.Quit();
#endif
    }
}
