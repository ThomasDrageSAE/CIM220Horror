using System.Collections;
using UnityEngine;

public class RemoteGameOverUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    private bool subscribed;
    private bool gameOverShown;

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForNetworkBattery());
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private IEnumerator WaitForNetworkBattery()
    {
        while (NetworkBatteryManager.Instance == null)
            yield return null;

        Subscribe();
        CheckBattery(NetworkBatteryManager.Instance.batteryPercent.Value);
    }

    private void Subscribe()
    {
        if (subscribed)
            return;

        NetworkBatteryManager.Instance.batteryPercent.OnValueChanged += OnBatteryChanged;
        subscribed = true;
    }

    private void Unsubscribe()
    {
        if (!subscribed)
            return;

        if (NetworkBatteryManager.Instance != null)
            NetworkBatteryManager.Instance.batteryPercent.OnValueChanged -= OnBatteryChanged;

        subscribed = false;
    }

    private void OnBatteryChanged(int oldValue, int newValue)
    {
        CheckBattery(newValue);
    }

    private void CheckBattery(int batteryValue)
    {
        if (gameOverShown)
            return;

        if (batteryValue <= 0)
        {
            gameOverShown = true;

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            PlayerInputLock.SetLocked(true);
        }
    }
}