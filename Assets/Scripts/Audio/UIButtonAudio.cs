using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private UIButtonSoundSet soundSet;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(HandleClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button == null || !button.interactable)
            return;

        if (UIAudioManager.Instance != null)
            UIAudioManager.Instance.PlayHover(soundSet);
    }

    private void HandleClick()
    {
        if (UIAudioManager.Instance == null)
            return;

        if (button != null && button.interactable)
            UIAudioManager.Instance.PlayClick(soundSet);
        else
            UIAudioManager.Instance.PlayDisabled(soundSet);
    }
}