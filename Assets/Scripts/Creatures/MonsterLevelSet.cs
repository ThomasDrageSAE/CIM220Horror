using UnityEngine;

[CreateAssetMenu(fileName = "MonsterLevelSet", menuName = "Monsters/Monster Level Set")]
public class MonsterLevelSet : ScriptableObject
{
    public int levelNumber;
    public MonsterData[] possibleMonsters;
}