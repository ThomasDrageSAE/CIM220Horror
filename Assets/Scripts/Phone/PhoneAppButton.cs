using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneAppButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PhoneController.PhoneApp appToOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerInputLock.IsLocked)
            return;
        
        if (PhoneController.Instance != null)
            PhoneController.Instance.OpenApp(appToOpen);
    }
}