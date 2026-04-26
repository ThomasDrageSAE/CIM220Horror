using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookLayout_TextImage : BookSpreadLayout
{
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private Image rightImage;
    [SerializeField] private TextMeshProUGUI rightCaption;

    public override void Apply(BookSpreadData data)
    {
        if (leftText != null)
            leftText.text = data.textA;

        if (rightImage != null)
        {
            rightImage.sprite = data.imageA;
            rightImage.enabled = data.imageA != null;
        }

        if (rightCaption != null)
            rightCaption.text = data.textB;
    }
}