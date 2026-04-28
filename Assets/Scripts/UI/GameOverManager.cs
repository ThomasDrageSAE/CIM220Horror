using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image gameOverImage;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private bool gameOver;

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        PlayerInputLock.SetLocked(true);

        MonsterData monster = encounterManager != null ? encounterManager.CurrentMonster : null;
        
        if (monster != null && monster.gameOverSound != null && GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.Play(monster.gameOverSound);
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverImage != null && monster != null && monster.gameOverImage != null)
        {
            gameOverImage.sprite = monster.gameOverImage;
            gameOverImage.enabled = true;
        }

        if (gameOverText != null)
        {
            if (monster != null && monster.gameOverDialogue != null && monster.gameOverDialogue.Length > 0)
                gameOverText.text = monster.gameOverDialogue[0];
            else
                gameOverText.text = "The phone dies. You are left in the dark.";
        }

        Debug.Log("[GameOver] Battery died.");
    }
}