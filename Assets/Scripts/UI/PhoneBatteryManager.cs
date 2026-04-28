using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBatteryManager : MonoBehaviour
{
    [Header("Battery")]
    [SerializeField] private int maxBattery = 100;
    [SerializeField] private int currentBattery = 100;
    [SerializeField] private int wrongChoicePenalty = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI batteryText;
    [SerializeField] private Image batteryFillImage;
    
    [Header("Game Over")]
    [SerializeField] private GameOverManager gameOverManager;

    public int CurrentBattery => currentBattery;
    public bool IsEmpty => currentBattery <= 0;

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

        if (currentBattery <= 0 && gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
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

        if (NetworkBatteryManager.Instance != null)
            NetworkBatteryManager.Instance.SetBattery(currentBattery);
    }
}