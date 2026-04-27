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

        if (phoneTimeManager != null)
            phoneTimeManager.SyncTime();

        if (encounterManager != null)
        {
            bool defeated = encounterManager.TryDefeatMonster(MonsterDefeatType.SyncTime);

            Debug.Log(defeated
                ? "[SettingsApp] Monster defeated by syncing time."
                : "[SettingsApp] Time synced, but wrong monster. Battery drained.");
        }
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