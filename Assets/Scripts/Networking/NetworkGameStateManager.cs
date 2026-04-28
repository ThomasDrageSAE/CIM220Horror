using Unity.Netcode;
using UnityEngine;

public class NetworkGameStateManager : NetworkBehaviour
{
    public static NetworkGameStateManager Instance;

    public NetworkVariable<bool> gameWon = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Awake()
    {
        Instance = this;
    }

    public void SetGameWon()
    {
        if (IsServer)
            gameWon.Value = true;
        else
            SetGameWonServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetGameWonServerRpc()
    {
        gameWon.Value = true;
    }
}