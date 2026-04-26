using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;

    [SerializeField] private AudioSource oneShotSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Play(GameSoundSet soundSet)
    {
        if (soundSet == null || oneShotSource == null)
            return;

        AudioClip clip = soundSet.GetRandomClip();
        if (clip == null)
            return;

        oneShotSource.pitch = soundSet.GetPitch();
        oneShotSource.PlayOneShot(clip, soundSet.volume);
    }
}