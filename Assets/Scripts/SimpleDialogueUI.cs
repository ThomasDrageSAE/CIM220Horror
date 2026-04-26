using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SimpleDialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typewriter")]
    [SerializeField] private float characterDelay = 0.03f;

    [Header("Dialogue Entry Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] dialogueEntrySounds;
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    public Action OnDialogueFinished;

    private string[] currentLines;
    private int currentIndex;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool dialogueActive;

    public bool DialogueActive => dialogueActive;

    private void Awake()
    {
        if (dialogueWindow != null)
            dialogueWindow.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";
    }

    public void ShowLines(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentIndex = 0;
        dialogueActive = true;

        PlayerInputLock.SetLocked(true);

        if (dialogueWindow != null)
            dialogueWindow.SetActive(true);

        ShowCurrentLine();
    }

    private void Update()
    {
        if (!dialogueActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
                FinishCurrentLineInstantly();
            else
                NextLine();
        }
    }

    private void ShowCurrentLine()
    {
        if (currentLines == null || currentIndex >= currentLines.Length)
            return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        PlayDialogueEntrySound();

        typingCoroutine = StartCoroutine(TypeLine(currentLines[currentIndex]));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;

        if (dialogueText != null)
            dialogueText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (dialogueText != null)
                dialogueText.text += line[i];

            yield return new WaitForSeconds(characterDelay);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void PlayDialogueEntrySound()
    {
        if (audioSource == null)
            return;

        if (dialogueEntrySounds == null || dialogueEntrySounds.Length == 0)
            return;

        AudioClip clip = dialogueEntrySounds[UnityEngine.Random.Range(0, dialogueEntrySounds.Length)];

        if (clip == null)
            return;

        audioSource.pitch = randomizePitch ? UnityEngine.Random.Range(minPitch, maxPitch) : 1f;
        audioSource.PlayOneShot(clip);
        audioSource.pitch = 1f;
    }

    private void FinishCurrentLineInstantly()
    {
        if (currentLines == null || currentIndex >= currentLines.Length)
            return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (dialogueText != null)
            dialogueText.text = currentLines[currentIndex];

        isTyping = false;
    }

    private void NextLine()
    {
        currentIndex++;

        if (currentLines == null || currentIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        currentLines = null;
        currentIndex = 0;
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (dialogueText != null)
            dialogueText.text = "";

        if (dialogueWindow != null)
            dialogueWindow.SetActive(false);

        PlayerInputLock.SetLocked(false);
        OnDialogueFinished?.Invoke();
    }
}