using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monsters/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Basic Info")]
    public string monsterName;
    [TextArea(3, 10)] public string description;
    public Sprite monsterImage;

    [Header("Defeat")]
    public MonsterDefeatType defeatType = MonsterDefeatType.None;

    [Header("Background")]
    public Sprite backgroundImage;
    
    [Header("Flashlight Settings")]
    public int requiredFlashCount = 1;

    [Header("Dialogue")]
    [TextArea(2, 6)] public string[] introDialogue;
    [TextArea(2, 6)] public string[] fightDialogue;
    [TextArea(2, 6)] public string[] defeatDialogue;

    [Header("Display Settings")]
    public bool useCustomImageSize = false;
    public Vector2 customImageSize = new Vector2(500f, 500f);
    public Vector2 imagePositionOffset = Vector2.zero;
    public bool preserveAspect = true;
    
    [Header("Temperature")]
    public bool causesTemperatureDrop = false;
    
    public MonsterIdleMotion.MotionStyle motionStyle = MonsterIdleMotion.MotionStyle.Float;
    public bool useGlitchMotion = false;
    
    [Header("Game Over")]
    public Sprite gameOverImage;
    [TextArea(2, 6)] public string[] gameOverDialogue;
    
    [Header("Monster Audio")]
    public GameSoundSet introSound;
    public GameSoundSet defeatSound;
    public GameSoundSet gameOverSound;
    
    [Header("Hints")]
    [TextArea(2, 6)] public string[] hintDialogue;
}