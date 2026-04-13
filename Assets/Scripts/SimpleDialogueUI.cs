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

        OnDialogueFinished?.Invoke();
    }
}