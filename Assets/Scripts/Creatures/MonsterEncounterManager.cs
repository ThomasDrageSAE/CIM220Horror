using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterEncounterManager : MonoBehaviour
{
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
    private bool currentMonsterDefeated = false;

    public MonsterData CurrentMonster => currentMonster;
    public bool CurrentMonsterDefeated => currentMonsterDefeated;
    public int CurrentLevel => currentLevel;

    private void Start()
    {
        StartLevel(currentLevel);
    }

    public void StartLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        currentMonsterDefeated = false;

        MonsterLevelSet levelSet = GetLevelSet(levelNumber);

        if (levelSet == null)
        {
            Debug.LogWarning("No MonsterLevelSet found for level " + levelNumber);
            currentMonster = null;
            RefreshUI();
            return;
        }

        if (levelSet.possibleMonsters == null || levelSet.possibleMonsters.Length == 0)
        {
            Debug.LogWarning("Level " + levelNumber + " has no possible monsters assigned.");
            currentMonster = null;
            RefreshUI();
            return;
        }

        int randomIndex = Random.Range(0, levelSet.possibleMonsters.Length);
        currentMonster = levelSet.possibleMonsters[randomIndex];

        Debug.Log("Level " + levelNumber + " selected monster: " + currentMonster.monsterName);

        RefreshUI();
        PlaySpawnSound();
        ShowIntroDialogue();
    }

    public void MarkCurrentMonsterDefeated()
    {
        if (currentMonster == null || currentMonsterDefeated)
            return;

        currentMonsterDefeated = true;

        PlayDefeatSound();
        ShowDefeatDialogue();
        RefreshUI();
    }

    public void SetMonsterDefeated(bool defeated)
    {
        currentMonsterDefeated = defeated;

        if (defeated)
        {
            PlayDefeatSound();
            ShowDefeatDialogue();
        }

        RefreshUI();
    }

    public void NextLevel()
    {
        StartLevel(currentLevel + 1);
    }

    public void ShowFightDialogue()
    {
        if (currentMonster == null || dialogueManager == null)
            return;

        dialogueManager.ShowLines(currentMonster.fightDialogue);
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
        {
            monsterNameText.text = currentMonster != null
                ? currentMonster.monsterName
                : "No Monster";
        }

        if (backgroundManager != null)
        {
            Sprite bg = currentMonster != null ? currentMonster.backgroundImage : null;
            backgroundManager.SetBackground(bg);
        }
    }

    private void PlaySpawnSound()
    {
        if (currentMonster == null || currentMonster.spawnSound == null)
            return;

        //if (GameAudioManager.Instance != null)
            //GameAudioManager.Instance.Play(currentMonster.spawnSound);
    }

    private void PlayDefeatSound()
    {
        if (currentMonster == null || currentMonster.defeatSound == null)
            return;

        //if (GameAudioManager.Instance != null)
            //GameAudioManager.Instance.Play(currentMonster.defeatSound);
    }

    private void ShowIntroDialogue()
    {
        if (currentMonster == null || dialogueManager == null)
            return;

        dialogueManager.ShowLines(currentMonster.introDialogue);
    }

    private void ShowDefeatDialogue()
    {
        if (currentMonster == null || dialogueManager == null)
            return;

        dialogueManager.ShowLines(currentMonster.defeatDialogue);
    }
}