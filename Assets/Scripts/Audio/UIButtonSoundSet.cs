using UnityEngine;

[CreateAssetMenu(fileName = "UIButtonSoundSet", menuName = "Audio/UI Button Sound Set")]
public class UIButtonSoundSet : ScriptableObject
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip disabledSound;

    [Header("Pitch Variation")]
    public bool randomizePitch = false;
    public float minPitch = 0.98f;
    public float maxPitch = 1.02f;

    [Header("Volume")]
    [Range(0f, 1f)] public float volume = 1f;
}