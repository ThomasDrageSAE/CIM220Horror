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

    private bool isVisible = false;
    private Coroutine moveRoutine;

    private void Awake()
    {
        if (phoneRect == null)
            phoneRect = transform as RectTransform;

        // Start hidden
        phoneRect.anchoredPosition = hiddenPosition;

        UpdatePrompt();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePhone();
        }
    }

    public void TogglePhone()
    {
        isVisible = !isVisible;

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

        // Overshoot for snappy feel
        Vector2 overshootTarget = Vector2.Lerp(start, target, overshootAmount);

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            phoneRect.anchoredPosition = Vector2.Lerp(start, overshootTarget, t);
            yield return null;
        }

        // Snap back to final position
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

    private void UpdatePrompt()
    {
        if (promptObject != null)
            promptObject.SetActive(!isVisible);

        if (promptText != null)
            promptText.text = "Press P for phone";
    }
}