using UnityEngine;
using UnityEngine.EventSystems;

public class BookInteractable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BookData bookData;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Book clicked");

        if (BookViewerUI.Instance == null)
        {
            Debug.LogWarning("No BookViewerUI in scene");
            return;
        }

        BookViewerUI.Instance.OpenBook(bookData);
    }
}