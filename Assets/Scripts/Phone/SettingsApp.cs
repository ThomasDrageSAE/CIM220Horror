using UnityEngine;

public class SettingsApp : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PhoneTimeManager phoneTimeManager;
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("Visuals")]
    [SerializeField] private GameObject settingsPanel;

    private bool active;

    private void Awake()
    {
        SetVisuals(false);
        gameObject.SetActive(true);
    }

    public void Activate()
    {
        active = true;
        SetVisuals(true);
    }

    public void Deactivate()
    {
        active = false;
        SetVisuals(false);
    }

    public void PressSyncTime()
    {
        if (!active)
            return;

        if (encounterManager == null || encounterManager.CurrentMonster == null)
            return;

        // ❌ WRONG MONSTER → drain battery only
        if (encounterManager.CurrentMonster.defeatType != MonsterDefeatType.SyncTime)
        {
            Debug.Log("[SettingsApp] Sync pressed on wrong monster. Battery drained.");

            if (encounterManager.BatteryManager != null)
                encounterManager.BatteryManager.DrainWrongChoice();

            return;
        }

        
        if (phoneTimeManager != null)
            phoneTimeManager.SyncTime();

        bool defeated = encounterManager.TryDefeatMonster(MonsterDefeatType.SyncTime);

        Debug.Log(defeated
            ? "[SettingsApp] Time monster defeated by syncing time."
            : "[SettingsApp] Sync attempted but did not defeat monster.");
    }

    public void PressClose()
    {
        if (PhoneController.Instance != null)
            PhoneController.Instance.CloseCurrentApp();
        else
            Deactivate();
    }

    private void SetVisuals(bool show)
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(show);
    }
}