using Unity.Netcode;
using UnityEngine;

public class GameRolePresentation : MonoBehaviour
{
    [SerializeField] private GameObject playerOneRoot;
    [SerializeField] private GameObject playerTwoRoot;

    private void Start()
    {
        ApplyRole();
    }

    private void ApplyRole()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("GameRolePresentation: No NetworkManager.Singleton found.");
            return;
        }

        bool isHost = NetworkManager.Singleton.IsHost;
        bool isClientOnly = NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost;

        if (playerOneRoot != null)
            playerOneRoot.SetActive(isHost);

        if (playerTwoRoot != null)
            playerTwoRoot.SetActive(isClientOnly);

        Debug.Log($"GameRolePresentation: isHost={isHost}, isClientOnly={isClientOnly}");
    }
}