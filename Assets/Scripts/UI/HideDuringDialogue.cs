using UnityEngine;

public class HideDuringDialogue : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToHide;
    [SerializeField] private SimpleDialogueUI dialogueUI;

    private void Update()
    {
        if (dialogueUI == null || objectsToHide == null)
            return;

        bool shouldHide = dialogueUI.DialogueActive;

        for (int i = 0; i < objectsToHide.Length; i++)
        {
            if (objectsToHide[i] != null)
                objectsToHide[i].SetActive(!shouldHide);
        }
    }
}