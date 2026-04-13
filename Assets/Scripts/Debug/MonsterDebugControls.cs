using UnityEngine;

public class MonsterDebugControls : MonoBehaviour
{
    [SerializeField] private MonsterEncounterManager encounterManager;

    private void Update()
    {
        if (encounterManager == null)
            return;

        // F1 = defeat monster
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("DEBUG: Forcing monster defeat");
            encounterManager.MarkCurrentMonsterDefeated();
        }

        // Optional extras (very useful while testing)
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("DEBUG: Next level");
            encounterManager.NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("DEBUG: Restart level");
            encounterManager.StartLevel(encounterManager.CurrentLevel);
        }
    }
}