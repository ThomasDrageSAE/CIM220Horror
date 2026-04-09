using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    [SerializeField] private AudioSource audioSource;

    private const string UiSoundsKey = "UISoundsEnabled";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayHover(UIButtonSoundSet soundSet)
    {
        if (!CanPlay(soundSet) || soundSet.hoverSound == null)
            return;

        PlayClip(soundSet.hoverSound, soundSet);
    }

    public void PlayClick(UIButtonSoundSet soundSet)
    {
        if (!CanPlay(soundSet) || soundSet.clickSound == null)
            return;

        PlayClip(soundSet.clickSound, soundSet);
    }

    public void PlayDisabled(UIButtonSoundSet soundSet)
    {
        if (!CanPlay(soundSet) || soundSet.disabledSound == null)
            return;

        PlayClip(soundSet.disabledSound, soundSet);
    }

    private bool CanPlay(UIButtonSoundSet soundSet)
    {
        return audioSource != null &&
               soundSet != null &&
               PlayerPrefs.GetInt(UiSoundsKey, 1) == 1;
    }

    private void PlayClip(AudioClip clip, UIButtonSoundSet soundSet)
    {
        if (soundSet.randomizePitch)
            audioSource.pitch = Random.Range(soundSet.minPitch, soundSet.maxPitch);
        else
            audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip, soundSet.volume);
    }
}