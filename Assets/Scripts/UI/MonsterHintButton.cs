using UnityEngine;

public class MonsterHintButton : MonoBehaviour
{
    [SerializeField] private MonsterEncounterManager encounterManager;

    public void PressHint()
    {
        if (encounterManager == null)
            return;

        encounterManager.ShowCurrentMonsterHint();
    }
}