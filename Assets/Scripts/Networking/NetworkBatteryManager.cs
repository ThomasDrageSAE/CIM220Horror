using Unity.Netcode;
using UnityEngine;

public class NetworkBatteryManager : NetworkBehaviour
{
    public static NetworkBatteryManager Instance;

    public NetworkVariable<int> batteryPercent = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Awake()
    {
        Instance = this;
    }

    public void SetBattery(int value)
    {
        value = Mathf.Clamp(value, 0, 100);

        if (IsServer)
            batteryPercent.Value = value;
        else
            SetBatteryServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetBatteryServerRpc(int value)
    {
        batteryPercent.Value = Mathf.Clamp(value, 0, 100);
    }
}