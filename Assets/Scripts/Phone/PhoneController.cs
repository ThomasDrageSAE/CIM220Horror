using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public enum PhoneApp { None, Camera }

    [Header("App References")]
    public CameraApp cameraApp;

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
        _activeApp = app;

        if (homeScreenRoot != null)
            homeScreenRoot.SetActive(false);

        switch (app)
        {
            case PhoneApp.Camera:
                if (cameraApp != null)
                    cameraApp.Activate();
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
        }

        _activeApp = PhoneApp.None;

        if (homeScreenRoot != null)
            homeScreenRoot.SetActive(true);
    }
}