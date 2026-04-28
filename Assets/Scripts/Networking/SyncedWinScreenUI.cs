using System.Collections;
using UnityEngine;

public class SyncedWinScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private bool subscribed;

    private void Awake()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForGameStateManager());
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private IEnumerator WaitForGameStateManager()
    {
        while (NetworkGameStateManager.Instance == null)
            yield return null;

        Subscribe();
        UpdateWinScreen(NetworkGameStateManager.Instance.gameWon.Value);
    }

    private void Subscribe()
    {
        if (subscribed)
            return;

        NetworkGameStateManager.Instance.gameWon.OnValueChanged += OnGameWonChanged;
        subscribed = true;
    }

    private void Unsubscribe()
    {
        if (!subscribed)
            return;

        if (NetworkGameStateManager.Instance != null)
            NetworkGameStateManager.Instance.gameWon.OnValueChanged -= OnGameWonChanged;

        subscribed = false;
    }

    private void OnGameWonChanged(bool oldValue, bool newValue)
    {
        UpdateWinScreen(newValue);
    }

    private void UpdateWinScreen(bool won)
    {
        if (!won)
            return;

        if (winPanel != null)
            winPanel.SetActive(true);

        PlayerInputLock.SetLocked(true);
    }
}