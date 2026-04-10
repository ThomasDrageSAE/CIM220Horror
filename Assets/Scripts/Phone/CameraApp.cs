using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraApp : MonoBehaviour
{
    [Header("Phone Movement")]
    public RectTransform phoneRectTransform;
    public Canvas parentCanvas;

    [Header("Return Settings")]
    [Tooltip("How fast the phone returns to start position")]
    public float returnSpeed = 5f;

    [Header("Hide Icons")]
    public GameObject[] appIcons;

    [Header("Viewfinder")]
    public Collider2D viewfinderCollider;

    [Header("Optional Visuals")]
    public CanvasGroup phoneCanvasGroup;
    [Range(0f, 1f)] public float activeBodyAlpha = 0.85f;

    [Header("Ghost Detection")]
    public LayerMask ghostLayerMask;
    public string ghostTag = "Ghost";

    [Header("Photo Flash")]
    public Image flashOverlay;
    public float flashDuration = 0.25f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shutterSound;
    public AudioClip ghostCapturedSound;

    public System.Action<bool, GameObject> OnPhotoTaken;

    private bool _active;
    private float _originalBodyAlpha = 1f;
    private Vector2 startPosition;
    private Coroutine returnRoutine;

    private void Awake()
    {
        if (phoneRectTransform == null)
            phoneRectTransform = transform as RectTransform;

        if (parentCanvas == null)
            parentCanvas = GetComponentInParent<Canvas>();

        // SAVE START POSITION
        startPosition = phoneRectTransform.anchoredPosition;

        if (phoneCanvasGroup != null)
            _originalBodyAlpha = phoneCanvasGroup.alpha;

        gameObject.SetActive(true);
        SetVisuals(false);
    }

    private void Update()
    {
        if (!_active)
            return;

        HandleMovement();
        HandleInput();
    }

    public void Activate()
    {
        _active = true;
        SetVisuals(true);

        // Hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Stop any return animation if active
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
    }

    public void Deactivate()
    {
        _active = false;
        SetVisuals(false);

        // Show cursor again
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Smooth return
        if (returnRoutine != null)
            StopCoroutine(returnRoutine);

        returnRoutine = StartCoroutine(ReturnToStart());
    }

    private void HandleMovement()
    {
        if (phoneRectTransform == null || parentCanvas == null)
            return;

        RectTransform canvasRect = parentCanvas.transform as RectTransform;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera,
            out localPoint))
        {
            // INSTANT movement (no lag)
            phoneRectTransform.anchoredPosition = localPoint;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PhoneController.Instance != null)
                PhoneController.Instance.CloseCurrentApp();
            else
                Deactivate();

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TakePhoto();
        }
    }

    private void TakePhoto()
    {
        PlaySound(shutterSound);

        GameObject capturedGhost = FindGhostInViewfinder();
        bool ghostCaptured = capturedGhost != null;

        if (ghostCaptured)
        {
            PlaySound(ghostCapturedSound);
            Debug.Log($"[CameraApp] Ghost captured: {capturedGhost.name}");
        }

        OnPhotoTaken?.Invoke(ghostCaptured, capturedGhost);

        StartCoroutine(FlashRoutine());
    }

    private GameObject FindGhostInViewfinder()
    {
        if (viewfinderCollider == null)
            return null;

        Bounds b = viewfinderCollider.bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(b.center, b.size, 0f, ghostLayerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(ghostTag))
                return hit.gameObject;
        }

        return null;
    }

    private IEnumerator FlashRoutine()
    {
        if (flashOverlay == null)
            yield break;

        float half = flashDuration * 0.5f;
        float t = 0f;

        while (t < half)
        {
            t += Time.deltaTime;
            SetFlashAlpha(Mathf.Lerp(0f, 1f, t / half));
            yield return null;
        }

        t = 0f;

        while (t < half)
        {
            t += Time.deltaTime;
            SetFlashAlpha(Mathf.Lerp(1f, 0f, t / half));
            yield return null;
        }

        SetFlashAlpha(0f);
    }

    private void SetFlashAlpha(float alpha)
    {
        if (flashOverlay == null)
            return;

        Color c = flashOverlay.color;
        c.a = alpha;
        flashOverlay.color = c;
    }

    private IEnumerator ReturnToStart()
    {
        Vector2 current = phoneRectTransform.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            phoneRectTransform.anchoredPosition = Vector2.Lerp(current, startPosition, t);
            yield return null;
        }

        phoneRectTransform.anchoredPosition = startPosition;
    }

    private void SetVisuals(bool show)
    {
        if (phoneCanvasGroup != null)
            phoneCanvasGroup.alpha = show ? activeBodyAlpha : _originalBodyAlpha;

        if (viewfinderCollider != null)
            viewfinderCollider.enabled = show;

        foreach (GameObject icon in appIcons)
        {
            if (icon != null)
                icon.SetActive(!show);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}