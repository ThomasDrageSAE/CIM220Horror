using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UVApp : MonoBehaviour
{
    [Header("Monster Defeat")]
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("UV Visual")]
    [SerializeField] private GameObject uvLightObject;
    [SerializeField] private Image uvOverlayImage;

    [Header("Timing")]
    [SerializeField] private float activeDuration = 0.6f;
    [SerializeField] private float closeDelay = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip uvOnSound;

    private bool active;

    private void Awake()
    {
        SetUVVisual(false);
        gameObject.SetActive(true);
    }

    public void Activate()
    {
        if (active)
            return;

        active = true;
        StartCoroutine(UVRoutine());
    }

    public void Deactivate()
    {
        active = false;
        SetUVVisual(false);
    }

    private IEnumerator UVRoutine()
    {
        PlaySound(uvOnSound);
        SetUVVisual(true);

        yield return new WaitForSeconds(activeDuration);

        if (encounterManager != null)
        {
            bool defeated = encounterManager.TryDefeatMonster(MonsterDefeatType.UV);

            Debug.Log(defeated
                ? "[UVApp] Monster defeated with UV."
                : "[UVApp] UV was wrong. Battery drained.");
        }
        else
        {
            Debug.LogWarning("[UVApp] No MonsterEncounterManager assigned.");
        }

        yield return new WaitForSeconds(closeDelay);

        if (PhoneController.Instance != null)
            PhoneController.Instance.CloseCurrentApp();
        else
            Deactivate();
    }

    private void SetUVVisual(bool show)
    {
        if (uvLightObject != null)
            uvLightObject.SetActive(show);

        if (uvOverlayImage != null)
        {
            uvOverlayImage.gameObject.SetActive(show);
            uvOverlayImage.enabled = show;

            Color c = uvOverlayImage.color;
            c.a = show ? 0.55f : 0f;
            uvOverlayImage.color = c;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}