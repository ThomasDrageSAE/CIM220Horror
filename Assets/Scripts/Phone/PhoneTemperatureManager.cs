using TMPro;
using UnityEngine;

public class PhoneTemperatureManager : MonoBehaviour
{
    [Header("Temperature")]
    [SerializeField] private float normalTemperature = 21f;
    [SerializeField] private float ghostTemperature = 4f;
    [SerializeField] private float changeSpeed = 4f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI temperatureText;

    private float currentTemperature;
    private float targetTemperature;

    private void Start()
    {
        currentTemperature = normalTemperature;
        targetTemperature = normalTemperature;
        RefreshUI();
    }

    private void Update()
    {
        currentTemperature = Mathf.Lerp(
            currentTemperature,
            targetTemperature,
            Time.deltaTime * changeSpeed
        );

        RefreshUI();
    }

    public void SetNormalTemperature()
    {
        targetTemperature = normalTemperature;
    }

    public void SetGhostTemperature()
    {
        targetTemperature = ghostTemperature;
    }

    private void RefreshUI()
    {
        if (temperatureText != null)
            temperatureText.text = currentTemperature.ToString("0.0") + "°C";
    }
}