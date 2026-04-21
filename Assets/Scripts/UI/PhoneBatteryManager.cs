using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBatteryManager : MonoBehaviour
{
    [SerializeField] private int maxBattery = 100;
    [SerializeField] private int currentBattery = 100;
    [SerializeField] private int wrongChoicePenalty = 10;

    [Header("Optional UI")]
    [SerializeField] private TextMeshProUGUI batteryText;
    [SerializeField] private Image batteryFillImage;

    public int CurrentBattery => currentBattery;
    public int MaxBattery => maxBattery;

    private void Start()
    {
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
        RefreshUI();
    }

    public void DrainWrongChoice()
    {
        DrainBattery(wrongChoicePenalty);
    }

    public void DrainBattery(int amount)
    {
        currentBattery = Mathf.Clamp(currentBattery - Mathf.Abs(amount), 0, maxBattery);
        RefreshUI();
    }

    public void RechargeBattery(int amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + Mathf.Abs(amount), 0, maxBattery);
        RefreshUI();
    }

    public float GetBatteryPercent01()
    {
        if (maxBattery <= 0)
            return 0f;

        return (float)currentBattery / maxBattery;
    }

    private void RefreshUI()
    {
        if (batteryText != null)
            batteryText.text = currentBattery + "%";

        if (batteryFillImage != null)
            batteryFillImage.fillAmount = GetBatteryPercent01();
    }
}