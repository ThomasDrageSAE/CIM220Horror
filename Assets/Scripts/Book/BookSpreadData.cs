using UnityEngine;

[System.Serializable]
public class BookSpreadData
{
    public BookSpreadLayout layout;

    [TextArea(3, 10)] public string textA;
    [TextArea(3, 10)] public string textB;
    [TextArea(3, 10)] public string textC;
    [TextArea(3, 10)] public string textD;

    public Sprite imageA;
    public Sprite imageB;
    public Sprite imageC;
    public Sprite imageD;
}