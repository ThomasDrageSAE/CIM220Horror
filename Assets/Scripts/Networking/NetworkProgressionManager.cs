using Unity.Netcode;
using UnityEngine;

public class NetworkProgressionManager : NetworkBehaviour
{
    public static NetworkProgressionManager Instance;

    public NetworkVariable<int> progressionIndex = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Awake()
    {
        Instance = this;
    }

    public void AdvanceProgression()
    {
        if (IsServer)
        {
            progressionIndex.Value++;
        }
        else
        {
            AdvanceProgressionServerRpc();
        }
    }

    public void SetProgression(int value)
    {
        if (IsServer)
        {
            progressionIndex.Value = value;
        }
        else
        {
            SetProgressionServerRpc(value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AdvanceProgressionServerRpc()
    {
        progressionIndex.Value++;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetProgressionServerRpc(int value)
    {
        progressionIndex.Value = value;
    }
}