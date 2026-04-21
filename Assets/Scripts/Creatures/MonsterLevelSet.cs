using UnityEngine;

[CreateAssetMenu(fileName = "MonsterLevelSet", menuName = "Monsters/Monster Level Set")]
public class MonsterLevelSet : ScriptableObject
{
    public int levelNumber;
    public MonsterData[] possibleMonsters;

    [TextArea(2, 6)]
    public string[] betweenFightDialogue;

    [Header("Ending")]
    public bool isEndingLevel;
    public Sprite endingBackground;

    [TextArea(2, 8)]
    public string[] endingDialogue;
}