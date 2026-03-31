using UnityEngine;

[CreateAssetMenu(fileName = "NewBookData", menuName = "Books/Book Data")]
public class BookData : ScriptableObject
{
    public string bookTitle;
    public BookSpreadData[] spreads;
}