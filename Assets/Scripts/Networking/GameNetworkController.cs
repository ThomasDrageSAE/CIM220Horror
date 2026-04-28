using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkController : NetworkBehaviour
{
    public static GameNetworkController Instance;

    public NetworkVariable<int> connectedPlayers = new NetworkVariable<int>(0);
    public NetworkVariable<bool> hostReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> clientReady = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("GNC: No NetworkManager.Singleton");
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        if (IsServer)
            RefreshConnectedPlayers();
    }

    public override void OnNetworkDespawn()
    {
        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
            RefreshConnectedPlayers();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (!IsServer)
            return;

        RefreshConnectedPlayers();

        if (clientId == NetworkManager.ServerClientId)
            hostReady.Value = false;
        else
            clientReady.Value = false;
    }

    private void RefreshConnectedPlayers()
    {
        if (!IsServer || NetworkManager.Singleton == null)
            return;

        connectedPlayers.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;

        if (senderId == NetworkManager.ServerClientId)
            hostReady.Value = true;
        else
            clientReady.Value = true;

        TryStartGame();
    }

    private void TryStartGame()
    {
        if (!IsServer)
            return;

        if (connectedPlayers.Value >= 2 && hostReady.Value && clientReady.Value)
        {
            gameStarted.Value = true;

            NetworkManager.Singleton.SceneManager.LoadScene("Game1", LoadSceneMode.Single);
        }
    }
}