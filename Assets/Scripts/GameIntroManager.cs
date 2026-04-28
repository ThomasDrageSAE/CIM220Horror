using System.Collections;
using UnityEngine;

public class GameIntroManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private MonsterDialogueManager dialogueManager;

    [TextArea(3, 8)]
    [SerializeField] private string[] introDialogue;

    [Header("How To Play")]
    [SerializeField] private GameObject howToPanel;

    [Header("Flow")]
    [SerializeField] private MonsterEncounterManager encounterManager;
    [SerializeField] private SimpleDialogueUI dialogueUI;
    [SerializeField] private ScreenFadeUI screenFadeUI;

    [Header("Timing")]
    [SerializeField] private float delayAfterFadeOut = 0.25f;
    [SerializeField] private float delayBeforeFadeIn = 0.2f;
    
    [SerializeField] private HideDuringTutorial tutorialVisibility;

    private void Start()
    {
        ShowHowTo();
    }

    private void ShowHowTo()
    {
        PlayerInputLock.SetLocked(true);

        if (howToPanel != null)
            howToPanel.SetActive(true);
        
        if (tutorialVisibility != null)
            tutorialVisibility.HideObjects();
    }

    public void CloseHowTo()
    {
        if (howToPanel != null)
            howToPanel.SetActive(false);

        StartCoroutine(BeginIntroSequence());
    }

    private IEnumerator BeginIntroSequence()
    {
        bool fadeOutDone = false;

        if (screenFadeUI != null)
            screenFadeUI.FadeOutToBlack(() => fadeOutDone = true);
        else
            fadeOutDone = true;

        while (!fadeOutDone)
            yield return null;

        yield return new WaitForSeconds(delayAfterFadeOut);

        StartIntroDialogue();
    }

    private void StartIntroDialogue()
    {
        if (dialogueManager == null || introDialogue == null || introDialogue.Length == 0)
        {
            FinishIntroAndStartGame();
            return;
        }

        if (dialogueUI != null)
            dialogueUI.OnDialogueFinished += OnIntroDialogueFinished;

        dialogueManager.ShowLines(introDialogue);
    }

    private void OnIntroDialogueFinished()
    {
        if (dialogueUI != null)
            dialogueUI.OnDialogueFinished -= OnIntroDialogueFinished;

        StartCoroutine(FinishIntroSequence());
    }

    private IEnumerator FinishIntroSequence()
    {
        yield return new WaitForSeconds(delayBeforeFadeIn);

        bool fadeInDone = false;

        if (screenFadeUI != null)
            screenFadeUI.FadeInFromBlack(() => fadeInDone = true);
        else
            fadeInDone = true;

        while (!fadeInDone)
            yield return null;

        FinishIntroAndStartGame();
    }

    private void FinishIntroAndStartGame()
    {
        PlayerInputLock.SetLocked(false);

        if (tutorialVisibility != null)
            tutorialVisibility.ShowObjects();

        if (encounterManager != null)
            encounterManager.StartLevel(1);
    }
}