using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallApp : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("Visuals")]
    [SerializeField] private GameObject callPanel;
    [SerializeField] private Button muteButton;
    [SerializeField] private TextMeshProUGUI muteButtonText;
    [SerializeField] private TextMeshProUGUI callTimeText;

    [Header("Mute Settings")]
    [SerializeField] private float requiredMuteSeconds = 4f;

    [Header("Audio Optional")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip muteClickSound;
    
    [Header("Dialogue")]
    [SerializeField] private MonsterEncounterManager encounterManagerForDialogue;
    [TextArea(2, 6)] [SerializeField] private string[] wrongMuteDialogue;
    [TextArea(2, 6)] [SerializeField] private string[] correctMuteDialogue;

    private bool active;
    private bool muted;
    private bool muteLocked;
    private float callTimer;
    private Coroutine muteRoutine;

    private void Awake()
    {
        SetVisuals(false);
        UpdateMuteButton();
    }

    private void Update()
    {
        callTimer += Time.deltaTime;

        if (active)
            UpdateCallTimer();
    }

    public void Activate()
    {
        active = true;
        SetVisuals(true);
        UpdateMuteButton();
        UpdateCallTimer(); 
    }

    public void Deactivate()
    {
        active = false;
        muted = false;

        if (muteRoutine != null)
        {
            StopCoroutine(muteRoutine);
            muteRoutine = null;
        }

        SetVisuals(false);
        UpdateMuteButton();
    }

    public void PressMute()
    {
        if (!active)
            return;

        if (muteLocked)
        {
            Debug.Log("[CallApp] Mute already used successfully.");
            return;
        }

        if (encounterManager == null || encounterManager.CurrentMonster == null)
            return;

        if (encounterManager.CurrentMonster.defeatType != MonsterDefeatType.Silence)
        {
            Debug.Log("[CallApp] Wrong monster for mute. Battery drained.");

            encounterManager.ShowInteractionDialogue(wrongMuteDialogue);
            encounterManager.TryDefeatMonster(MonsterDefeatType.Silence);

            return;
        }

        muted = true;
        PlaySound(muteClickSound);
        UpdateMuteButton();

        encounterManager.ShowInteractionDialogue(correctMuteDialogue);

        if (muteRoutine != null)
            StopCoroutine(muteRoutine);

        muteRoutine = StartCoroutine(MuteHoldRoutine());
    }

    private IEnumerator MuteHoldRoutine()
    {
        Debug.Log("[CallApp] Muted. Waiting for silence...");

        yield return new WaitForSeconds(requiredMuteSeconds);

        if (!muted || encounterManager == null)
            yield break;

        bool defeated = encounterManager.TryDefeatMonster(MonsterDefeatType.Silence);

        if (defeated)
        {
            Debug.Log("[CallApp] Monster defeated by silence.");
            muteLocked = true;
            UpdateMuteButton();

            yield return new WaitForSeconds(0.2f);

            if (PhoneController.Instance != null)
                PhoneController.Instance.CloseCurrentApp();
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
        if (callPanel != null)
            callPanel.SetActive(show);
    }

    private void UpdateMuteButton()
    {
        if (muteButton != null)
            muteButton.interactable = !muteLocked;

        if (muteButtonText != null)
        {
            if (muteLocked)
                muteButtonText.text = "Muted";
            else
                muteButtonText.text = muted ? "Muted..." : "Mute Call";
        }
    }

    private void UpdateCallTimer()
    {
        if (callTimeText == null)
            return;

        int totalSeconds = Mathf.FloorToInt(callTimer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        callTimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}