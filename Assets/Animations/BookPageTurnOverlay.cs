using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BookPageTurnOverlay : MonoBehaviour
{
    [SerializeField] private Image overlayImage;
    [SerializeField] private Animator animator;
    [SerializeField] private float animationDuration = 0.35f;

    private bool isPlaying;
    public bool IsPlaying => isPlaying;

    private void Awake()
    {
        if (overlayImage != null)
            overlayImage.enabled = false;
    }

    public void PlayForward(Action onMidpoint = null, Action onComplete = null)
    {
        StartCoroutine(PlayRoutine("PlayForward", onMidpoint, onComplete));
    }

    public void PlayBackward(Action onMidpoint = null, Action onComplete = null)
    {
        StartCoroutine(PlayRoutine("PlayBackward", onMidpoint, onComplete));
    }

    private IEnumerator PlayRoutine(string triggerName, Action onMidpoint, Action onComplete)
    {
        if (isPlaying || overlayImage == null || animator == null)
            yield break;

        isPlaying = true;
        overlayImage.enabled = true;

        animator.ResetTrigger("PlayForward");
        animator.ResetTrigger("PlayBackward");
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(animationDuration * 0.5f);

        onMidpoint?.Invoke();

        yield return new WaitForSeconds(animationDuration * 0.5f);

        overlayImage.enabled = false;
        isPlaying = false;

        onComplete?.Invoke();
    }
}