using UnityEngine;

public class FightPromptUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject promptObject;
    [SerializeField] private SimpleDialogueUI dialogueUI;
    [SerializeField] private MonsterEncounterManager encounterManager;

    private void Update()
    {
        if (promptObject == null || dialogueUI == null || encounterManager == null)
            return;

        bool shouldShow =
            encounterManager.IsInFight &&
            !dialogueUI.DialogueActive;

        promptObject.SetActive(shouldShow);
    }
}