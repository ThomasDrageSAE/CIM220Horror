using UnityEngine;


public class PhoneController : MonoBehaviour
{
    public enum PhoneApp { None, Camera }

    [Header("App References")]
    public CameraApp cameraApp;

    [Header("Settings")]
    [Tooltip("Layer mask for clicking the phone when idle")]
    public LayerMask phoneLayerMask;

    private PhoneApp _activeApp = PhoneApp.None;
    private Camera _mainCamera;

    public static PhoneController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (_activeApp == PhoneApp.None)
        {
            HandleIdleInput();
        }
        else
        {
            // Each app handles its own Escape / input
        }
    }

    private void HandleIdleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero, Mathf.Infinity, phoneLayerMask);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OpenApp(PhoneApp.Camera);
            }
        }
    }

    public void OpenApp(PhoneApp app)
    {
        _activeApp = app;

        switch (app)
        {
            case PhoneApp.Camera:
                cameraApp.Activate();
                break;
        }
    }

    public void CloseCurrentApp()
    {
        switch (_activeApp)
        {
            case PhoneApp.Camera:
                cameraApp.Deactivate();
                break;
        }

        _activeApp = PhoneApp.None;
    }
}