using System.Collections;
using UnityEngine;

public class ScreenShakeUI : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float strength = 20f;

    private Vector2 originalPosition;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (targetRect == null)
            targetRect = transform as RectTransform;

        if (targetRect != null)
            originalPosition = targetRect.anchoredPosition;
    }

    public void Shake()
    {
        if (targetRect == null)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            Vector2 offset = Random.insideUnitCircle * strength;
            targetRect.anchoredPosition = originalPosition + offset;

            yield return null;
        }

        targetRect.anchoredPosition = originalPosition;
        shakeRoutine = null;
    }
}