using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject lobbyRoot;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI readyButtonText;

    private bool localReadyPressed;

    private void Update()
    {
        if (lobbyRoot == null || !lobbyRoot.activeSelf)
            return;

        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        {
            statusText.text = "Starting network...";
            return;
        }

        if (GameNetworkController.Instance == null)
        {
            statusText.text = "Waiting for network controller...";
            return;
        }

        if (GameNetworkController.Instance.gameStarted.Value)
        {
            statusText.text = "Loading game...";
            readyButton.gameObject.SetActive(false);
            return;
        }

        bool twoPlayersConnected = GameNetworkController.Instance.connectedPlayers.Value >= 2;
        string roleName = NetworkManager.Singleton.IsHost ? "Host" : "Client";

        statusText.text =
            $"Role: {roleName}\n" +
            $"Connected Players: {GameNetworkController.Instance.connectedPlayers.Value}/2\n" +
            $"Host Ready: {GameNetworkController.Instance.hostReady.Value}\n" +
            $"Client Ready: {GameNetworkController.Instance.clientReady.Value}\n\n" +
            (twoPlayersConnected
                ? (localReadyPressed ? "Waiting for other player..." : "Press Ready")
                : "Waiting for second player...");

        readyButton.interactable = twoPlayersConnected && !localReadyPressed;

        if (!twoPlayersConnected)
            readyButtonText.text = "Waiting...";
        else if (localReadyPressed)
            readyButtonText.text = "Ready";
        else
            readyButtonText.text = "Ready Up";
    }

    public void PressReady()
    {
        if (localReadyPressed)
            return;

        if (GameNetworkController.Instance == null)
            return;

        if (GameNetworkController.Instance.connectedPlayers.Value < 2)
            return;

        GameNetworkController.Instance.SetReadyServerRpc();
        localReadyPressed = true;
        readyButton.interactable = false;
    }
}