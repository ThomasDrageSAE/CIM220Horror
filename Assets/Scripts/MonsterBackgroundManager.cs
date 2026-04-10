using UnityEngine;
using UnityEngine.UI;

public class MonsterBackgroundManager : MonoBehaviour
{
    [SerializeField] private Image backgroundImageDisplay;

    public void SetBackground(Sprite newBackground)
    {
        if (backgroundImageDisplay == null)
            return;

        if (newBackground == null)
        {
            backgroundImageDisplay.sprite = null;
            backgroundImageDisplay.color = new Color(1f, 1f, 1f, 0f);
            return;
        }

        backgroundImageDisplay.sprite = newBackground;
        backgroundImageDisplay.color = Color.white;
        backgroundImageDisplay.preserveAspect = true;
    }
}