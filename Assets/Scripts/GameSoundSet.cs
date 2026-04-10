using UnityEngine;

[CreateAssetMenu(fileName = "GameSoundSet", menuName = "Audio/Game Sound Set")]
public class GameSoundSet : ScriptableObject
{
    public AudioClip[] clips;

    [Range(0f, 1f)] public float volume = 1f;

    public bool randomizePitch = true;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[Random.Range(0, clips.Length)];
    }

    public float GetPitch()
    {
        if (!randomizePitch)
            return 1f;

        return Random.Range(minPitch, maxPitch);
    }
}