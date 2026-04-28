using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemoteBatteryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI batteryText;
    [SerializeField] private Image batteryFillImage;

    private bool subscribed;

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
        UpdateUI(NetworkBatteryManager.Instance.batteryPercent.Value);
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
        UpdateUI(newValue);
    }

    private void UpdateUI(int value)
    {
        value = Mathf.Clamp(value, 0, 100);

        if (batteryText != null)
            batteryText.text = value + "%";

        if (batteryFillImage != null)
            batteryFillImage.fillAmount = value / 100f;
    }
}