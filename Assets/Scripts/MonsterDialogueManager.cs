using UnityEngine;

public class MonsterDialogueManager : MonoBehaviour
{
    [SerializeField] private SimpleDialogueUI dialogueUI;
    [SerializeField] private MonsterEncounterManager encounterManager;

    private void Start()
    {
        if (dialogueUI != null)
            dialogueUI.OnDialogueFinished += HandleDialogueFinished;
    }

    public void ShowLines(string[] lines)
    {
        if (dialogueUI == null || lines == null || lines.Length == 0)
            return;

        dialogueUI.ShowLines(lines);
    }

    private void HandleDialogueFinished()
    {
        if (encounterManager == null)
            return;

        // If monster is not defeated yet → show fight dialogue
        if (!encounterManager.CurrentMonsterDefeated)
        {
            encounterManager.ShowFightDialogue();
        }
    }

    public bool IsDialogueActive()
    {
        return dialogueUI != null && dialogueUI.DialogueActive;
    }
}