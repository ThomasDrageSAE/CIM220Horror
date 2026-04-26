using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public enum PhoneApp { None, Camera, UV }

    [Header("App References")]
    public CameraApp cameraApp;
    public UVApp uvApp;

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
        }

        _activeApp = PhoneApp.None;

        if (homeScreenRoot != null)
            homeScreenRoot.SetActive(true);
    }
}