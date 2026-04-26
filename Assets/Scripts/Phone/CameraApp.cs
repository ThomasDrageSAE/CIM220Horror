using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraApp : MonoBehaviour
{
    [Header("Phone Movement")]
    [SerializeField] private RectTransform phoneRectTransform; // Assign PhoneRoot here
    [SerializeField] private Canvas parentCanvas;

    [Header("Return Settings")]
    [SerializeField] private float returnSpeed = 5f;

    [Header("Visuals")]
    [SerializeField] private GameObject phoneFill;
    [SerializeField] private GameObject[] appIcons;

    [Header("Viewfinder")]
    [SerializeField] private Collider2D viewfinderCollider;

    [Header("Photo Flash")]
    [SerializeField] private Image flashOverlay;
    [SerializeField] private float flashDuration = 0.25f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shutterSound;

    [Header("Monster Defeat")]
    [SerializeField] private MonsterEncounterManager encounterManager;

    private bool _active;
    private Vector2 _startPosition;
    private Coroutine _returnRoutine;
    public System.Action<bool, GameObject> OnPhotoTaken;

    private void Awake()
    {
        if (phoneRectTransform == null)
        {
            Debug.LogWarning("[CameraApp] phoneRectTransform is not assigned. Drag PhoneRoot into this field.");
            return;
        }

        if (parentCanvas == null)
            parentCanvas = phoneRectTransform.GetComponentInParent<Canvas>();

        _startPosition = phoneRectTransform.anchoredPosition;
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
        }
    }

    public void Deactivate()
    {
        _active = false;
        SetVisuals(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (_returnRoutine != null)
            StopCoroutine(_returnRoutine);

        _returnRoutine = StartCoroutine(ReturnToStart());
    }

    private void HandleMovement()
    {
        if (phoneRectTransform == null || parentCanvas == null)
            return;

        RectTransform canvasRect = parentCanvas.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera,
            out Vector2 localPoint))
        {
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
            TakePhoto();
    }

    private void TakePhoto()
    {
        PlaySound(shutterSound);
        StartCoroutine(FlashRoutine());
        StartCoroutine(PhotoSequenceRoutine());
    }

    private IEnumerator PhotoSequenceRoutine()
    {
        if (encounterManager == null)
        {
            Debug.LogWarning("[CameraApp] No MonsterEncounterManager assigned.");
            yield break;
        }

        if (encounterManager.CurrentMonster == null)
        {
            Debug.LogWarning("[CameraApp] No current monster.");
            yield break;
        }

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.12f);
        Time.timeScale = 1f;

        bool defeated = encounterManager.TryDefeatMonster(MonsterDefeatType.Camera);

        Debug.Log(defeated
            ? "[CameraApp] Monster defeated with camera."
            : "[CameraApp] Wrong method. Battery drained.");

        OnPhotoTaken?.Invoke(defeated, null);
        yield return new WaitForSeconds(0.2f);

        if (PhoneController.Instance != null)
            PhoneController.Instance.CloseCurrentApp();
    }

    private IEnumerator FlashRoutine()
    {
        if (flashOverlay == null)
            yield break;

        float half = flashDuration * 0.5f;
        float t = 0f;

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            SetFlashAlpha(Mathf.Lerp(0f, 1f, t / half));
            yield return null;
        }

        t = 0f;

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
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
        if (phoneRectTransform == null)
            yield break;

        Vector2 current = phoneRectTransform.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            phoneRectTransform.anchoredPosition = Vector2.Lerp(current, _startPosition, t);
            yield return null;
        }

        phoneRectTransform.anchoredPosition = _startPosition;
    }

    private void SetVisuals(bool show)
    {
        if (phoneFill != null)
            phoneFill.SetActive(!show);

        if (viewfinderCollider != null)
            viewfinderCollider.enabled = show;

        for (int i = 0; i < appIcons.Length; i++)
        {
            if (appIcons[i] != null)
                appIcons[i].SetActive(!show);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}