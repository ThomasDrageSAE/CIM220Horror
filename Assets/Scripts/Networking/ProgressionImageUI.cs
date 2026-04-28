using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionImageUI : MonoBehaviour
{
    [SerializeField] private Image progressionImage;

    [Header("Images")]
    [SerializeField] private Sprite[] progressionSprites;

    private bool subscribed;

    private void Awake()
    {
        if (progressionImage == null)
            progressionImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForProgressionManager());
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private IEnumerator WaitForProgressionManager()
    {
        while (NetworkProgressionManager.Instance == null)
            yield return null;

        Subscribe();
        UpdateImage(NetworkProgressionManager.Instance.progressionIndex.Value);
    }

    private void Subscribe()
    {
        if (subscribed)
            return;

        NetworkProgressionManager.Instance.progressionIndex.OnValueChanged += OnProgressionChanged;
        subscribed = true;
    }

    private void Unsubscribe()
    {
        if (!subscribed)
            return;

        if (NetworkProgressionManager.Instance != null)
            NetworkProgressionManager.Instance.progressionIndex.OnValueChanged -= OnProgressionChanged;

        subscribed = false;
    }

    private void OnProgressionChanged(int oldValue, int newValue)
    {
        UpdateImage(newValue);
    }

    private void UpdateImage(int index)
    {
        if (progressionImage == null)
        {
            Debug.LogWarning("[ProgressionImageUI] No Image assigned.");
            return;
        }

        if (progressionSprites == null || progressionSprites.Length == 0)
        {
            Debug.LogWarning("[ProgressionImageUI] No progression sprites assigned.");
            return;
        }

        index = Mathf.Clamp(index, 0, progressionSprites.Length - 1);

        Debug.Log("[ProgressionImageUI] Updating image to index: " + index);

        progressionImage.sprite = progressionSprites[index];
    }
}