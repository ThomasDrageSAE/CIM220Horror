using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CameraApp : MonoBehaviour
{

    [Header("Phone Movement")]
    [Tooltip("The transform that moves to follow the mouse.")]
    public Transform phoneTransform;

    [Tooltip("How quickly the phone lerps to the mouse position (0 = instant).")]
    [Range(0f, 30f)]
    public float followSpeed = 20f;

    [Tooltip("Hide app icons when in camera mode")]
    public GameObject[] appIcons;

    [Tooltip("Z depth in world-space for the phone while in camera mode.")]
    public float phoneDepth = 0f;

    [Header("Viewfinder")]
    [Tooltip("A Collider2D (trigger) centred on the phone's transparent window. " +
             "Ghosts inside this area count as photographed.")]
    public Collider2D viewfinderCollider;

    [Tooltip("The empty GameObject for the location of the phone")]
    public Transform homePosition;

    [Tooltip("Optional: a SpriteRenderer whose alpha is set to make the centre transparent.")]
    public SpriteRenderer phoneBodyRenderer;

    [Tooltip("Alpha for the phone body while camera is active (lower = more transparent centre illusion).")]
    [Range(0f, 1f)]
    public float activeBodyAlpha = 0.85f;

    [Header("Ghost Detection")]
    [Tooltip("Layer(s) that ghosts live on.")]
    public LayerMask ghostLayerMask;

    [Tooltip("Tag used on ghost/spirit GameObjects.")]
    public string ghostTag = "Ghost";

    [Header("Photo Flash")]
    [Tooltip("A full-screen UI Image (white, starts at alpha 0) used for the flash effect.")]
    public Image flashOverlay;

    [Tooltip("How long the flash lasts in seconds.")]
    public float flashDuration = 0.25f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shutterSound;
    public AudioClip ghostCapturedSound;

    public System.Action<bool, GameObject> OnPhotoTaken;


    private bool _active;
    private Camera _mainCamera;
    private float _originalBodyAlpha = 1f;


    void Awake()
    {
        _mainCamera = Camera.main;

        if (phoneTransform == null)
            phoneTransform = transform;

        if (phoneBodyRenderer != null)
            _originalBodyAlpha = phoneBodyRenderer.color.a;

        gameObject.SetActive(true);
        SetVisuals(false);
    }

    private void OnMouseDown()
    {
        if (!_active)
            Activate();
    }

    void Update()
    {
        if (!_active) return;

        HandleMovement();
        HandleInput();
    }

    public void Activate()
    {
        _active = true;
        SetVisuals(true);
    }

    public void Deactivate()
    {
        _active = false;
        SetVisuals(false);
        phoneTransform.position = homePosition.position;
    }

    private void HandleMovement()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = _mainCamera.nearClipPlane + phoneDepth;
        Vector3 targetWorld = _mainCamera.ScreenToWorldPoint(mouseScreen);
        targetWorld.z = phoneDepth;

        if (followSpeed <= 0f)
        {
            phoneTransform.position = targetWorld;
        }
        else
        {
            phoneTransform.position = Vector3.Lerp(
                phoneTransform.position,
                targetWorld,
                Time.deltaTime * followSpeed
            );
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        else
        {
            Debug.Log("[CameraApp] Photo taken — no ghost detected.");
        }

        OnPhotoTaken?.Invoke(ghostCaptured, capturedGhost);

        StartCoroutine(FlashRoutine());
    }


    private GameObject FindGhostInViewfinder()
    {
        if (viewfinderCollider == null) return null;

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
        if (flashOverlay == null) yield break;

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
        if (flashOverlay == null) return;
        Color c = flashOverlay.color;
        c.a = alpha;
        flashOverlay.color = c;
    }

    private void SetVisuals(bool show)
    {
        if (phoneBodyRenderer != null)
        {
            Color c = phoneBodyRenderer.color;
            c.a = show ? activeBodyAlpha : _originalBodyAlpha;
            phoneBodyRenderer.color = c;
        }

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