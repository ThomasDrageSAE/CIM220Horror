using UnityEngine;
using UnityEngine.UI;

public class BookViewerUI : MonoBehaviour
{
    public static BookViewerUI Instance;

    [SerializeField] private GameObject bookUIRoot;
    [SerializeField] private Transform spreadParent;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private BookPageTurnOverlay pageTurnOverlay;

    private BookData currentBook;
    private int currentSpreadIndex;
    private GameObject currentSpreadObject;

    private void Awake()
    {
        Instance = this;

        if (bookUIRoot != null)
            bookUIRoot.SetActive(false);

        if (prevButton != null)
            prevButton.onClick.AddListener(PreviousSpread);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextSpread);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseBook);
    }

    public void OpenBook(BookData bookData)
    {
        if (bookData == null || bookData.spreads == null || bookData.spreads.Length == 0)
            return;

        currentBook = bookData;
        currentSpreadIndex = 0;

        if (bookUIRoot != null)
            bookUIRoot.SetActive(true);

        RefreshUI();
    }

    public void CloseBook()
    {
        currentBook = null;
        currentSpreadIndex = 0;

        ClearCurrentSpread();

        if (bookUIRoot != null)
            bookUIRoot.SetActive(false);
    }

    public void NextSpread()
    {
        if (currentBook == null)
            return;

        if (currentSpreadIndex >= currentBook.spreads.Length - 1)
            return;

        if (pageTurnOverlay != null && !pageTurnOverlay.IsPlaying)
        {
            if (spreadParent != null)
                spreadParent.gameObject.SetActive(false);

            pageTurnOverlay.PlayForward(
                () =>
                {
                    currentSpreadIndex++;
                    RefreshUI();
                },
                () =>
                {
                    if (spreadParent != null)
                        spreadParent.gameObject.SetActive(true);
                }
            );
        }
        else if (pageTurnOverlay == null)
        {
            currentSpreadIndex++;
            RefreshUI();
        }
    }

    public void PreviousSpread()
    {
        if (currentBook == null)
            return;

        if (currentSpreadIndex <= 0)
            return;

        if (pageTurnOverlay != null && !pageTurnOverlay.IsPlaying)
        {
            if (spreadParent != null)
                spreadParent.gameObject.SetActive(false);

            pageTurnOverlay.PlayBackward(
                () =>
                {
                    currentSpreadIndex--;
                    RefreshUI();
                },
                () =>
                {
                    if (spreadParent != null)
                        spreadParent.gameObject.SetActive(true);
                }
            );
        }
        else if (pageTurnOverlay == null)
        {
            currentSpreadIndex--;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (currentBook == null) return;

        ClearCurrentSpread();

        BookSpreadData spreadData = currentBook.spreads[currentSpreadIndex];

        if (spreadData.layout != null)
        {
            currentSpreadObject = Instantiate(spreadData.layout.gameObject, spreadParent);
            BookSpreadLayout layoutInstance = currentSpreadObject.GetComponent<BookSpreadLayout>();

            if (layoutInstance != null)
                layoutInstance.Apply(spreadData);
        }

        if (prevButton != null)
            prevButton.interactable = currentSpreadIndex > 0;

        if (nextButton != null)
            nextButton.interactable = currentSpreadIndex < currentBook.spreads.Length - 1;
    }

    private void ClearCurrentSpread()
    {
        if (currentSpreadObject != null)
            Destroy(currentSpreadObject);
    }
}