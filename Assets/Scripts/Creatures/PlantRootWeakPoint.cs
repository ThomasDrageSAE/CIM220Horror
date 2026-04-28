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

    [Header("Shrink Feedback")]
    [SerializeField] private bool shrinkOnClick = true;
    [SerializeField] private float clickedScale = 0.75f;
    [SerializeField] private float shrinkSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        if (rootImage == null)
            rootImage = GetComponent<Image>();

        ResetRoot();
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * shrinkSpeed);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clicked)
            return;

        if (PlayerInputLock.IsLocked)
            return;

        clicked = true;

        if (shrinkOnClick)
            targetScale = originalScale * clickedScale;
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
        targetScale = originalScale;
    
        if (rootImage != null)
            rootImage.color = normalColor;
    }
}