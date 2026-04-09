using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuRoot;
    [SerializeField] private GameObject settingsRoot;
    [SerializeField] private GameObject lobbyRoot;

    public void PressStart()
    {
        if (AutoBootNetwork.Instance != null)
            AutoBootNetwork.Instance.StartNetworkFromArgs();

        if (mainMenuRoot != null)
            mainMenuRoot.SetActive(false);

        if (settingsRoot != null)
            settingsRoot.SetActive(false);

        if (lobbyRoot != null)
            lobbyRoot.SetActive(true);
    }

    public void OpenSettings()
    {
        if (settingsRoot != null)
            settingsRoot.SetActive(true);

        if (mainMenuRoot != null)
            mainMenuRoot.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsRoot != null)
            settingsRoot.SetActive(false);

        if (mainMenuRoot != null)
            mainMenuRoot.SetActive(true);
    }

    public void PressQuit()
    {
        Application.Quit();
    }
}