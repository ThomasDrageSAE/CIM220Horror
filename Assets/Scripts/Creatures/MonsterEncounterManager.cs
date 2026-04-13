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
        Defeat
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

    public MonsterData CurrentMonster => currentMonster;
    public bool CurrentMonsterDefeated => currentMonsterDefeated;

    // ✅ ADDED THIS LINE (fix for your error)
    public int CurrentLevel => currentLevel;

    private void Start()
    {
        StartLevel(currentLevel);
    }

    public void StartLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        currentMonsterDefeated = false;
        fightDialogueShown = false;
        monsterRevealed = false;
        currentDialoguePhase = DialoguePhase.None;

        MonsterLevelSet levelSet = GetLevelSet(levelNumber);

        if (levelSet == null || levelSet.possibleMonsters == null || levelSet.possibleMonsters.Length == 0)
        {
            Debug.LogWarning("No valid monster level set found for level " + levelNumber);
            currentMonster = null;
            RefreshUI();
            return;
        }

        int randomIndex = Random.Range(0, levelSet.possibleMonsters.Length);
        currentMonster = levelSet.possibleMonsters[randomIndex];

        HideMonsterUI();
        ShowIntroDialogue();
    }

    public void MarkCurrentMonsterDefeated()
    {
        if (currentMonster == null || currentMonsterDefeated)
            return;

        currentMonsterDefeated = true;
        currentDialoguePhase = DialoguePhase.Defeat;

        ShowDefeatDialogue();
        RefreshUI();
    }

    public void ShowFightDialogue()
    {
        if (currentMonster == null || dialogueManager == null || fightDialogueShown)
            return;

        if (currentMonster.fightDialogue == null || currentMonster.fightDialogue.Length == 0)
            return;

        currentDialoguePhase = DialoguePhase.Fight;
        fightDialogueShown = true;
        dialogueManager.ShowLines(currentMonster.fightDialogue);
    }

    public void OnDialogueSequenceFinished()
    {
        if (currentMonster == null)
            return;

        if (currentDialoguePhase == DialoguePhase.Intro)
        {
            RevealMonsterUI();
            ShowFightDialogue();
            return;
        }

        currentDialoguePhase = DialoguePhase.None;
    }

    public void ShowInteractionDialogue(string[] lines)
    {
        if (dialogueManager == null || lines == null || lines.Length == 0)
            return;

        dialogueManager.ShowLines(lines);
    }

    public void NextLevel()
    {
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
            }
        }

        if (monsterNameText != null)
            monsterNameText.text = currentMonster != null ? currentMonster.monsterName : "No Monster";

        if (backgroundManager != null)
            backgroundManager.SetBackground(currentMonster != null ? currentMonster.backgroundImage : null);
    }

    private void RevealMonsterUI()
    {
        monsterRevealed = true;
        RefreshUI();
    }

    private void HideMonsterUI()
    {
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
            return;

        dialogueManager.ShowLines(currentMonster.defeatDialogue);
    }
}