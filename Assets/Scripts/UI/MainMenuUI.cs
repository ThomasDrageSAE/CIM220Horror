using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuRoot;
    [SerializeField] private GameObject lobbyRoot;

    public void PressPlay()
    {
        if (AutoBootNetwork.Instance != null)
            AutoBootNetwork.Instance.StartNetworkFromArgs();

        if (mainMenuRoot != null)
            mainMenuRoot.SetActive(false);

        if (lobbyRoot != null)
            lobbyRoot.SetActive(true);
    }

    public void PressQuit()
    {
        Application.Quit();
    }
}