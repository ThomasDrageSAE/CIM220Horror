using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightApp : MonoBehaviour
{
    [Header("Monster Defeat")]
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("Flashlight Visual")]
    [SerializeField] private GameObject flashlightBeamObject;
    [SerializeField] private Image flashlightOverlayImage;

    [Header("Flicker")]
    [SerializeField] private bool flickerOnActivate = true;
    [SerializeField] private int flickerCount = 3;
    [SerializeField] private float flickerInterval = 0.08f;

    [Header("Timing")]
    [SerializeField] private float activeDuration = 0.8f;
    [SerializeField] private float closeDelay = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flashlightOnSound;
    [SerializeField] private AudioClip flashlightOffSound;

    private bool active;

    private void Awake()
    {
        SetFlashlightVisual(false);
        gameObject.SetActive(true);
    }

    public void Activate()
    {
        if (active)
            return;

        active = true;
        StartCoroutine(FlashlightRoutine());
    }

    public void Deactivate()
    {
        active = false;
        SetFlashlightVisual(false);
        PlaySound(flashlightOffSound);
    }

    private IEnumerator FlashlightRoutine()
    {
        if (flickerOnActivate)
            yield return StartCoroutine(FlickerRoutine());

        PlaySound(flashlightOnSound);
        SetFlashlightVisual(true);

        yield return new WaitForSeconds(activeDuration);

        if (encounterManager != null)
        {
            bool defeated = encounterManager.TryFlashlight();

            Debug.Log(defeated
                ? "[FlashlightApp] Monster defeated with flashlight."
                : "[FlashlightApp] Flashlight used. Monster not defeated yet.");
        }
        else
        {
            Debug.LogWarning("[FlashlightApp] No MonsterEncounterManager assigned.");
        }

        SetFlashlightVisual(false);
        PlaySound(flashlightOffSound);

        yield return new WaitForSeconds(closeDelay);

        if (PhoneController.Instance != null)
            PhoneController.Instance.CloseCurrentApp();
        else
            Deactivate();
    }

    private IEnumerator FlickerRoutine()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            SetFlashlightVisual(true);
            yield return new WaitForSeconds(flickerInterval);
            SetFlashlightVisual(false);
            yield return new WaitForSeconds(flickerInterval);
        }
    }

    private void SetFlashlightVisual(bool show)
    {
        if (flashlightBeamObject != null)
            flashlightBeamObject.SetActive(show);

        if (flashlightOverlayImage != null)
        {
            flashlightOverlayImage.gameObject.SetActive(show);
            flashlightOverlayImage.enabled = show;

            Color c = flashlightOverlayImage.color;
            c.a = show ? 0.65f : 0f;
            flashlightOverlayImage.color = c;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}