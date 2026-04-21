using UnityEngine;

public class EndingGameStateHook : MonoBehaviour
{
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("Objects to disable when ending starts")]
    [SerializeField] private GameObject[] objectsToDisable;

    [Header("Objects to enable when ending finishes")]
    [SerializeField] private GameObject[] objectsToEnableOnEnd;

    private void OnEnable()
    {
        if (encounterManager != null)
        {
            encounterManager.OnEndingStarted += HandleEndingStarted;
            encounterManager.OnEndingFinished += HandleEndingFinished;
        }
    }

    private void OnDisable()
    {
        if (encounterManager != null)
        {
            encounterManager.OnEndingStarted -= HandleEndingStarted;
            encounterManager.OnEndingFinished -= HandleEndingFinished;
        }
    }

    private void HandleEndingStarted()
    {
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            if (objectsToDisable[i] != null)
                objectsToDisable[i].SetActive(false);
        }
    }

    private void HandleEndingFinished()
    {
        for (int i = 0; i < objectsToEnableOnEnd.Length; i++)
        {
            if (objectsToEnableOnEnd[i] != null)
                objectsToEnableOnEnd[i].SetActive(true);
        }
    }
}