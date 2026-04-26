using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneAppButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PhoneController.PhoneApp appToOpen;

    [Header("Input Lock")]
    [SerializeField] private bool ignoreInputLock = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Phone app icon clicked: " + appToOpen);

        if (PlayerInputLock.IsLocked && !ignoreInputLock)
        {
            Debug.Log("Phone app blocked because input is locked.");
            return;
        }

        if (PhoneController.Instance != null)
            PhoneController.Instance.OpenApp(appToOpen);
        else
            Debug.LogWarning("No PhoneController.Instance found.");
    }
}