using UnityEngine;

public class FlashlightAppIcon : MonoBehaviour
{
    public FlashlightApp FlashlightApp;

    private void OnMouseDown()
    {
        FlashlightApp.Activate();
    }
}