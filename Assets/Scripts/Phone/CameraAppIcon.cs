using UnityEngine;

public class CameraAppIcon : MonoBehaviour
{
    public CameraApp CameraApp;

    private void OnMouseDown()
    {
        CameraApp.Activate();
    }
}
