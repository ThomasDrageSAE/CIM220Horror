using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class AutoBootNetwork : MonoBehaviour
{
    public static AutoBootNetwork Instance;

    [SerializeField] private string hostIp = "127.0.0.1";
    [SerializeField] private ushort port = 7777;

    private bool started;

    private void Awake()
    {
        Instance = this;
    }

    public void StartNetworkFromArgs()
    {
        if (started)
            return;

        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("BOOT: No NetworkManager.Singleton found.");
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport == null)
        {
            Debug.LogError("BOOT: No UnityTransport found on NetworkManager object.");
            return;
        }

        bool startAsHost = false;
        string[] args = Environment.GetCommandLineArgs();

        foreach (string arg in args)
        {
            if (arg.Equals("-host", StringComparison.OrdinalIgnoreCase))
            {
                startAsHost = true;
                break;
            }
        }

        if (startAsHost)
        {
            transport.SetConnectionData("0.0.0.0", port);
            bool success = NetworkManager.Singleton.StartHost();
            Debug.Log("BOOT: StartHost = " + success);
        }
        else
        {
            transport.SetConnectionData(hostIp, port);
            bool success = NetworkManager.Singleton.StartClient();
            Debug.Log("BOOT: StartClient = " + success);
        }

        started = true;
    }
}