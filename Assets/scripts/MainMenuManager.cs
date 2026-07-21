using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    // Called by Start Button
    public void StartGame()
    {
        SceneManager.LoadScene("Game"); // Make sure your scene is named "Game"
    }

    // Called by Quit Button
    public void QuitGame()
    {
        Debug.Log("Quit pressed!");
        Application.Quit();

        // For Editor testing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void OpenOptions()
    {
        // 1. Disable the Main Menu buttons
        mainMenuPanel.SetActive(false);

        // 2. Enable the Options Menu buttons/sliders
        creditsPanel.SetActive(true);

        Debug.Log("Switched to Options Menu.");
    }
    public void ShowMainMenu()
    {
        // 1. Disable the Options Menu
        creditsPanel.SetActive(false);

        // 2. Enable the Main Menu
        mainMenuPanel.SetActive(true);

        Debug.Log("Switched to Main Menu.");
    }
}
