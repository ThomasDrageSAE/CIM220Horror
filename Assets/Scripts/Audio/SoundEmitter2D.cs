using UnityEngine;

public class SoundEmitter2D : MonoBehaviour
{
    [SerializeField] private GameSoundSet soundSet;

    [Header("Auto Play")]
    [SerializeField] private bool playOnEnable = false;

    private void OnEnable()
    {
        if (playOnEnable)
            Play();
    }

    public void Play()
    {
        if (GameAudioManager.Instance == null || soundSet == null)
            return;

        GameAudioManager.Instance.Play(soundSet);
    }
}