using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Settings")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool paused;

    private void Awake()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (paused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        paused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        paused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        if (Unity.Netcode.NetworkManager.Singleton != null)
            Unity.Netcode.NetworkManager.Singleton.Shutdown();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}