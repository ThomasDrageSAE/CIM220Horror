using System.Collections;
using UnityEngine;
using TMPro;

public class PhoneTogglePosition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform phoneRect;

    [Header("Prompt UI")]
    [SerializeField] private GameObject promptObject;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Positions")]
    [SerializeField] private Vector2 hiddenPosition;
    [SerializeField] private Vector2 visiblePosition;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private KeyCode toggleKey = KeyCode.P;

    [Header("Polish")]
    [SerializeField] private float overshootAmount = 1.1f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;
    private bool isVisible = false;
    private Coroutine moveRoutine;

    private void Awake()
    {
        if (phoneRect == null)
            phoneRect = transform as RectTransform;

        phoneRect.anchoredPosition = hiddenPosition;
        UpdatePrompt();
    }

    private void Update()
    {
        if (PlayerInputLock.IsLocked)
            return;

        if (Input.GetKeyDown(toggleKey))
        {
            TogglePhone();
        }
    }

    public void TogglePhone()
    {
        isVisible = !isVisible;

        PlayToggleSound(isVisible);

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        Vector2 target = isVisible ? visiblePosition : hiddenPosition;
        moveRoutine = StartCoroutine(MoveTo(target));

        UpdatePrompt();
    }

    private IEnumerator MoveTo(Vector2 target)
    {
        Vector2 start = phoneRect.anchoredPosition;
        float t = 0f;

        Vector2 overshootTarget = Vector2.Lerp(start, target, overshootAmount);

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            phoneRect.anchoredPosition = Vector2.Lerp(start, overshootTarget, t);
            yield return null;
        }

        float snapT = 0f;
        Vector2 current = phoneRect.anchoredPosition;

        while (snapT < 1f)
        {
            snapT += Time.deltaTime * moveSpeed;
            phoneRect.anchoredPosition = Vector2.Lerp(current, target, snapT);
            yield return null;
        }

        phoneRect.anchoredPosition = target;
    }

    private void PlayToggleSound(bool opening)
    {
        if (audioSource == null)
            return;

        AudioClip clip = opening ? openSound : closeSound;

        if (clip == null)
            return;

        audioSource.pitch = randomizePitch ? Random.Range(minPitch, maxPitch) : 1f;
        audioSource.PlayOneShot(clip);
        audioSource.pitch = 1f;
    }

    private void UpdatePrompt()
    {
        if (promptObject != null)
            promptObject.SetActive(!isVisible);

        if (promptText != null)
            promptText.text = "Press P for phone";
    }
}