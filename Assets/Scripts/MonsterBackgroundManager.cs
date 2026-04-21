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
            backgroundImageDisplay.color = Color.black;
            return;
        }

        backgroundImageDisplay.sprite = newBackground;
        backgroundImageDisplay.color = Color.white;
        backgroundImageDisplay.preserveAspect = true;
    }

    public void SetBlack()
    {
        if (backgroundImageDisplay == null)
            return;

        backgroundImageDisplay.sprite = null;
        backgroundImageDisplay.color = Color.black;
    }
}