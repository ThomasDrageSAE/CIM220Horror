using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float defaultFadeDuration = 0.75f;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }
    }

    public void FadeOutToBlack(System.Action onComplete = null)
    {
        StartFade(1f, defaultFadeDuration, onComplete);
    }

    public void FadeInFromBlack(System.Action onComplete = null)
    {
        StartFade(0f, defaultFadeDuration, onComplete);
    }

    public void SetBlackInstant()
    {
        if (fadeImage == null)
            return;

        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;
    }

    public void ClearInstant()
    {
        if (fadeImage == null)
            return;

        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
        fadeImage.gameObject.SetActive(false);
    }

    private void StartFade(float targetAlpha, float duration, System.Action onComplete)
    {
        if (fadeImage == null)
        {
            onComplete?.Invoke();
            return;
        }

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha, duration, onComplete));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, System.Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;

        if (Mathf.Approximately(targetAlpha, 0f))
            fadeImage.gameObject.SetActive(false);

        fadeRoutine = null;
        onComplete?.Invoke();
    }
}