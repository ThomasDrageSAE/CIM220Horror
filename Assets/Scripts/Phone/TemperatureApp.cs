using UnityEngine;

public class TemperatureApp : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject temperaturePanel;

    private bool active;

    private void Awake()
    {
        SetVisuals(false);
    }

    public void Activate()
    {
        active = true;
        SetVisuals(true);
    }

    public void Deactivate()
    {
        active = false;
        SetVisuals(false);
    }

    public void PressClose()
    {
        if (PhoneController.Instance != null)
            PhoneController.Instance.CloseCurrentApp();
        else
            Deactivate();
    }

    private void SetVisuals(bool show)
    {
        if (temperaturePanel != null)
            temperaturePanel.SetActive(show);
    }
}