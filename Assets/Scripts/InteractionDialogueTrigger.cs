using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionDialogueTrigger : MonoBehaviour, IPointerClickHandler
{
    [TextArea(2, 6)]
    [SerializeField] private string[] dialogueLines;

    [SerializeField] private MonsterEncounterManager encounterManager;
    [SerializeField] private bool onlyTriggerOnce = false;

    private bool hasTriggered;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onlyTriggerOnce && hasTriggered)
            return;

        if (encounterManager == null || dialogueLines == null || dialogueLines.Length == 0)
            return;

        encounterManager.ShowInteractionDialogue(dialogueLines);
        hasTriggered = true;
    }
}