using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public enum PhoneApp { None, Camera, UV, Flashlight, Settings }

    [Header("App References")]
    public CameraApp cameraApp;
    public UVApp uvApp;
    public FlashlightApp flashlightApp;
    public SettingsApp settingsApp;

    [Header("UI References")]
    public GameObject homeScreenRoot;

    private PhoneApp _activeApp = PhoneApp.None;

    public static PhoneController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_activeApp == PhoneApp.None)
        {
            // Idle state, waiting for app icons to open apps
        }
        else
        {
            // Each app handles its own Escape / input
        }
    }

    public void OpenApp(PhoneApp app)
    {
        Debug.Log("Opening app: " + app);

        _activeApp = app;

        if (homeScreenRoot != null)
            homeScreenRoot.SetActive(false);

        switch (app)
        {
            case PhoneApp.Camera:
                Debug.Log("Activating Camera app");
                if (cameraApp != null)
                    cameraApp.Activate();
                else
                    Debug.LogWarning("CameraApp is not assigned.");
                break;

            case PhoneApp.UV:
                Debug.Log("Activating UV app");
                if (uvApp != null)
                    uvApp.Activate();
                else
                    Debug.LogWarning("UVApp is not assigned.");
                break;

            case PhoneApp.Flashlight:
                Debug.Log("Activating Flashlight app");
                if (flashlightApp != null)
                    flashlightApp.Activate();
                else
                    Debug.LogWarning("FlashlightApp is not assigned.");
                break;
            case PhoneApp.Settings:
                Debug.Log("Activating Settings app");
                if (settingsApp != null)
                    settingsApp.Activate();
                break;
        }
        
    }

    public void CloseCurrentApp()
    {
        switch (_activeApp)
        {
            case PhoneApp.Camera:
                if (cameraApp != null)
                    cameraApp.Deactivate();
                break;

            case PhoneApp.UV:
                if (uvApp != null)
                    uvApp.Deactivate();
                break;

            case PhoneApp.Flashlight:
                if (flashlightApp != null)
                    flashlightApp.Deactivate();
                break;
            case PhoneApp.Settings:
                if (settingsApp != null)
                    settingsApp.Deactivate();
                break;
        }

        _activeApp = PhoneApp.None;

        if (homeScreenRoot != null)
            homeScreenRoot.SetActive(true);
    }
}