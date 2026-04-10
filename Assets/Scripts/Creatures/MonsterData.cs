using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monsters/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Basic Info")]
    public string monsterName;
    [TextArea(3, 10)] public string description;
    public Sprite monsterImage;

    [Header("Audio")]
    public GameSoundSet spawnSound;
    public GameSoundSet defeatSound;

    [Header("Background")]
    public Sprite backgroundImage;

    [Header("Dialogue")]
    [TextArea(2, 6)] public string[] introDialogue;
    [TextArea(2, 6)] public string[] fightDialogue;
    [TextArea(2, 6)] public string[] defeatDialogue;
}