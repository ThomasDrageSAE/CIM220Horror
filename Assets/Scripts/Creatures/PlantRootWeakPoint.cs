using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlantRootWeakPoint : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PlantRootPuzzleController puzzleController;
    [SerializeField] private Image rootImage;

    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color clickedColor = Color.gray;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    private bool clicked;

    private void Awake()
    {
        if (rootImage == null)
            rootImage = GetComponent<Image>();

        ResetRoot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clicked)
            return;

        if (PlayerInputLock.IsLocked)
            return;

        clicked = true;

        if (rootImage != null)
            rootImage.color = clickedColor;

        // 🔊 PLAY SOUND
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        if (puzzleController != null)
            puzzleController.RootClicked(this);
    }

    public void ResetRoot()
    {
        clicked = false;

        if (rootImage != null)
            rootImage.color = normalColor;
    }
}