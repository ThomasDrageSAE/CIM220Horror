using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookLayout_CollageText : BookSpreadLayout
{
    [SerializeField] private Image imageTopLeft;
    [SerializeField] private Image imageTopRight;
    [SerializeField] private Image imageBottomLeft;
    [SerializeField] private TextMeshProUGUI rightText;

    public override void Apply(BookSpreadData data)
    {
        SetImage(imageTopLeft, data.imageA);
        SetImage(imageTopRight, data.imageB);
        SetImage(imageBottomLeft, data.imageC);

        if (rightText != null)
            rightText.text = data.textA;
    }

    private void SetImage(Image target, Sprite sprite)
    {
        if (target == null)
            return;

        target.sprite = sprite;
        target.enabled = sprite != null;
    }
}