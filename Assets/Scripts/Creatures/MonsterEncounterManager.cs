using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterEncounterManager : MonoBehaviour
{
    private enum DialoguePhase
    {
        None,
        Intro,
        Fight,
        Defeat,
        BetweenFights,
        Ending
    }

    [Header("Level Data")]
    [SerializeField] private MonsterLevelSet[] levelSets;

    [Header("UI")]
    [SerializeField] private Image monsterImageDisplay;
    [SerializeField] private TextMeshProUGUI monsterNameText;

    [Header("Other Managers")]
    [SerializeField] private MonsterBackgroundManager backgroundManager;
    [SerializeField] private MonsterDialogueManager dialogueManager;

    [Header("Debug / Current State")]
    [SerializeField] private int currentLevel = 1;

    private MonsterData currentMonster;
    private bool currentMonsterDefeated;
    private DialoguePhase currentDialoguePhase = DialoguePhase.None;
    private bool fightDialogueShown;
    private bool monsterRevealed;
    private bool endingReached;
    private int currentFlashCount;
    
    [Header("Effects")]
    [SerializeField] private ScreenShakeUI screenShake;
    [SerializeField] private bool shakeOnFirstMonsterOnly = true;
    [SerializeField] private MonsterIdleMotion monsterMotion;
    
    [Header("Gameplay")]
    [SerializeField] private PhoneBatteryManager batteryManager;

    [SerializeField] private RectTransform monsterImageRect;
    [SerializeField] private Vector2 defaultImageSize = new Vector2(500f, 500f);
    [SerializeField] private Vector2 defaultImagePosition = Vector2.zero;
    private bool firstMonsterRevealDone;
    
    [Header("Phone Temperature")]
    [SerializeField] private PhoneTemperatureManager phoneTemperatureManager;
    public MonsterDefeatType CurrentDefeatType => currentMonster != null ? currentMonster.defeatType : MonsterDefeatType.None;

    public MonsterData CurrentMonster => currentMonster;
    public bool CurrentMonsterDefeated => currentMonsterDefeated;
    public int CurrentLevel => currentLevel;

    public bool EndingReached => endingReached;
    
    public System.Action OnEndingStarted;
    public System.Action OnEndingFinished;
    public PhoneBatteryManager BatteryManager => batteryManager;
    
    [SerializeField] private PhoneTimeManager phoneTimeManager;
    
    
    

    public void StartLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        currentMonsterDefeated = false;
        fightDialogueShown = false;
        monsterRevealed = false;
        currentDialoguePhase = DialoguePhase.None;
        currentFlashCount = 0;

        MonsterLevelSet levelSet = GetLevelSet(levelNumber);

        if (levelSet == null)
        {
            Debug.LogWarning("No level set found for level " + levelNumber);
            currentMonster = null;
            HideMonsterUI();

            if (backgroundManager != null)
                backgroundManager.SetBlack();

            return;
        }

        if (levelSet.isEndingLevel)
        {
            StartEnding(levelSet);
            return;
        }

        if (levelSet.possibleMonsters == null || levelSet.possibleMonsters.Length == 0)
        {
            Debug.LogWarning("No valid monster level set found for level " + levelNumber);
            currentMonster = null;
            HideMonsterUI();

            if (backgroundManager != null)
                backgroundManager.SetBlack();

            return;
        }

        int randomIndex = Random.Range(0, levelSet.possibleMonsters.Length);
        currentMonster = levelSet.possibleMonsters[randomIndex];

        HideMonsterUI();
        ShowIntroDialogue();
    }

    public void MarkCurrentMonsterDefeated()
    {
        if (currentMonster == null || currentMonsterDefeated || endingReached)
            return;

        currentMonsterDefeated = true;
        currentDialoguePhase = DialoguePhase.Defeat;

        ShowDefeatDialogue();
        RefreshUI();
        if (phoneTimeManager != null)
            phoneTimeManager.StartNormalTime();
        
        if (phoneTemperatureManager != null)
            phoneTemperatureManager.SetNormalTemperature();
    }

    public void ShowFightDialogue()
    {
        if (currentMonster == null || dialogueManager == null || fightDialogueShown || endingReached)
            return;

        if (currentMonster.fightDialogue == null || currentMonster.fightDialogue.Length == 0)
        {
            currentDialoguePhase = DialoguePhase.None;
            return;
        }

        currentDialoguePhase = DialoguePhase.Fight;
        fightDialogueShown = true;
        dialogueManager.ShowLines(currentMonster.fightDialogue);
    }

    public void OnDialogueSequenceFinished()
    {
        if (endingReached)
        {
            if (currentDialoguePhase == DialoguePhase.Ending)
            {
                currentDialoguePhase = DialoguePhase.None;
                OnEndingFinished?.Invoke();
            }

            return;
        }

        if (currentMonster == null && currentDialoguePhase != DialoguePhase.BetweenFights)
            return;

        switch (currentDialoguePhase)
        {
            case DialoguePhase.Intro:
                RevealMonsterUI();
                ShowFightDialogue();
                break;

            case DialoguePhase.Fight:
                currentDialoguePhase = DialoguePhase.None;
                break;

            case DialoguePhase.Defeat:
                StartBetweenFightTransition();
                break;

            case DialoguePhase.BetweenFights:
                GoToNextLevel();
                break;

            default:
                currentDialoguePhase = DialoguePhase.None;
                break;
        }
    }

    public void ShowInteractionDialogue(string[] lines)
    {
        if (dialogueManager == null || lines == null || lines.Length == 0 || endingReached)
            return;

        dialogueManager.ShowLines(lines);
    }

    public void NextLevel()
    {
        StartLevel(currentLevel + 1);
    }

    private void StartEnding(MonsterLevelSet endingLevel)
    {
        endingReached = true;
        currentMonster = null;
        HideMonsterUI();

        OnEndingStarted?.Invoke();

        if (backgroundManager != null)
        {
            if (endingLevel.endingBackground != null)
                backgroundManager.SetBackground(endingLevel.endingBackground);
            else
                backgroundManager.SetBlack();
        }

        if (dialogueManager != null &&
            endingLevel.endingDialogue != null &&
            endingLevel.endingDialogue.Length > 0)
        {
            currentDialoguePhase = DialoguePhase.Ending;
            dialogueManager.ShowLines(endingLevel.endingDialogue);
        }
        else
        {
            currentDialoguePhase = DialoguePhase.None;
            OnEndingFinished?.Invoke();
        }
    }

    private void StartBetweenFightTransition()
    {
        HideMonsterUI();

        if (backgroundManager != null)
            backgroundManager.SetBlack();

        MonsterLevelSet levelSet = GetLevelSet(currentLevel);

        if (levelSet != null &&
            levelSet.betweenFightDialogue != null &&
            levelSet.betweenFightDialogue.Length > 0 &&
            dialogueManager != null)
        {
            currentDialoguePhase = DialoguePhase.BetweenFights;
            dialogueManager.ShowLines(levelSet.betweenFightDialogue);
        }
        else
        {
            GoToNextLevel();
        }
    }

    private void GoToNextLevel()
    {
        currentDialoguePhase = DialoguePhase.None;

        if (NetworkProgressionManager.Instance != null)
            NetworkProgressionManager.Instance.AdvanceProgression();

        StartLevel(currentLevel + 1);
    }

    private MonsterLevelSet GetLevelSet(int levelNumber)
    {
        if (levelSets == null)
            return null;

        for (int i = 0; i < levelSets.Length; i++)
        {
            if (levelSets[i] != null && levelSets[i].levelNumber == levelNumber)
                return levelSets[i];
        }

        return null;
    }

    private void RefreshUI()
    {
        if (!monsterRevealed)
        {
            HideMonsterUI();
            return;
        }

        if (monsterImageDisplay != null)
        {
            if (currentMonster != null && currentMonster.monsterImage != null)
            {
                monsterImageDisplay.sprite = currentMonster.monsterImage;
                monsterImageDisplay.color = Color.white;
                monsterImageDisplay.enabled = true;
            }
            else
            {
                monsterImageDisplay.sprite = null;
                monsterImageDisplay.color = new Color(1f, 1f, 1f, 0f);
                monsterImageDisplay.enabled = false;
            }
        }
        
        if (monsterImageDisplay != null)
        {
            if (currentMonster != null && currentMonster.monsterImage != null)
            {
                monsterImageDisplay.sprite = currentMonster.monsterImage;
                monsterImageDisplay.color = Color.white;
                monsterImageDisplay.enabled = true;
                monsterImageDisplay.preserveAspect = currentMonster.preserveAspect;

                if (monsterImageRect != null)
                {
                    monsterImageRect.anchoredPosition = defaultImagePosition + currentMonster.imagePositionOffset;

                    if (currentMonster.useCustomImageSize)
                        monsterImageRect.sizeDelta = currentMonster.customImageSize;
                    else
                        monsterImageRect.sizeDelta = defaultImageSize;
                }
            }
            else
            {
                monsterImageDisplay.sprite = null;
                monsterImageDisplay.color = new Color(1f, 1f, 1f, 0f);
                monsterImageDisplay.enabled = false;

                if (monsterImageRect != null)
                {
                    monsterImageRect.anchoredPosition = defaultImagePosition;
                    monsterImageRect.sizeDelta = defaultImageSize;
                }
            }
        }

        if (monsterNameText != null)
            monsterNameText.text = currentMonster != null ? currentMonster.monsterName : "";

        if (backgroundManager != null)
            backgroundManager.SetBackground(currentMonster != null ? currentMonster.backgroundImage : null);
    }

    private void RevealMonsterUI()
    {
        monsterRevealed = true;
        RefreshUI();

        if (phoneTemperatureManager != null && currentMonster != null)
        {
            if (currentMonster.causesTemperatureDrop)
                phoneTemperatureManager.SetGhostTemperature();
            else
                phoneTemperatureManager.SetNormalTemperature();
        }
        
        if (monsterMotion != null && currentMonster != null)
        {
            monsterMotion.SetMotionStyle(currentMonster.motionStyle);
            monsterMotion.SetGlitch(currentMonster.useGlitchMotion);
        }

       
        if (screenShake != null)
        {
            if (!shakeOnFirstMonsterOnly || !firstMonsterRevealDone)
            {
                screenShake.Shake();
                firstMonsterRevealDone = true;
            }
        }
        if (phoneTimeManager != null && currentMonster != null)
        {
            if (currentMonster.defeatType == MonsterDefeatType.SyncTime)
                phoneTimeManager.StartFastTime();
            else
                phoneTimeManager.StartNormalTime();
        }
    }

    private void HideMonsterUI()
    {
        monsterRevealed = false;

        if (monsterImageDisplay != null)
        {
            monsterImageDisplay.sprite = null;
            monsterImageDisplay.color = new Color(1f, 1f, 1f, 0f);
            monsterImageDisplay.enabled = false;
        }

        if (monsterNameText != null)
            monsterNameText.text = "";
    }

    private void ShowIntroDialogue()
    {
        if (currentMonster == null || dialogueManager == null)
            return;

        if (currentMonster.introDialogue == null || currentMonster.introDialogue.Length == 0)
        {
            RevealMonsterUI();
            ShowFightDialogue();
            return;
        }

        currentDialoguePhase = DialoguePhase.Intro;
        dialogueManager.ShowLines(currentMonster.introDialogue);
    }

    private void ShowDefeatDialogue()
    {
        if (currentMonster == null || dialogueManager == null)
            return;

        if (currentMonster.defeatDialogue == null || currentMonster.defeatDialogue.Length == 0)
        {
            StartBetweenFightTransition();
            return;
        }

        dialogueManager.ShowLines(currentMonster.defeatDialogue);
    }
    public bool TryDefeatMonster(MonsterDefeatType attemptedType)
    {
        if (currentMonster == null)
            return false;

        if (currentMonsterDefeated)
            return false;

        if (!monsterRevealed)
            return false;

        bool correct = currentMonster.defeatType == attemptedType;

        if (correct)
        {
            MarkCurrentMonsterDefeated();
            return true;
        }

        if (batteryManager != null)
            batteryManager.DrainWrongChoice();

        return false;
    }
    
    public bool TryFlashlight()
    {
        if (currentMonster == null)
            return false;

        if (currentMonsterDefeated)
            return false;

        if (!monsterRevealed)
            return false;

        if (currentMonster.defeatType != MonsterDefeatType.Flash)
        {
            if (batteryManager != null)
                batteryManager.DrainWrongChoice();

            Debug.Log("[Flashlight] Wrong monster for flashlight.");
            return false;
        }

        currentFlashCount++;

        int requiredFlashes = Mathf.Max(1, currentMonster.requiredFlashCount);

        Debug.Log("[Flashlight] Count: " + currentFlashCount + "/" + requiredFlashes);

        if (monsterMotion != null)
            monsterMotion.ShrinkPulse(0.25f);
        else
            Debug.LogWarning("[Flashlight] No MonsterIdleMotion assigned.");

        if (currentFlashCount >= requiredFlashes)
        {
            MarkCurrentMonsterDefeated();
            return true;
        }

        return false;
    }
}