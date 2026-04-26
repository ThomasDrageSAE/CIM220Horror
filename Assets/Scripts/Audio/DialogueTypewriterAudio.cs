using UnityEngine;

public class DialogueTypewriterAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;

    [Header("Settings")]
    [SerializeField] private int playEveryNCharacters = 2;
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private int charCounter;

    public void ResetAudio()
    {
        charCounter = 0;
    }

    public void OnCharacterTyped(char c)
    {
        // Skip spaces so it doesn’t spam
        if (char.IsWhiteSpace(c))
            return;

        charCounter++;

        if (charCounter % playEveryNCharacters != 0)
            return;

        PlaySound();
    }

    private void PlaySound()
    {
        if (audioSource == null || typeSound == null)
            return;

        audioSource.pitch = randomizePitch
            ? Random.Range(minPitch, maxPitch)
            : 1f;

        audioSource.PlayOneShot(typeSound);
        audioSource.pitch = 1f;
    }
}