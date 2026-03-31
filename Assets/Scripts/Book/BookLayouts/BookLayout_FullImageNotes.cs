using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookLayout_FullImageNotes : BookSpreadLayout
{
    [SerializeField] private Image fullSpreadImage;
    [SerializeField] private TextMeshProUGUI noteTopLeft;
    [SerializeField] private TextMeshProUGUI noteTopRight;
    [SerializeField] private TextMeshProUGUI noteBottom;

    public override void Apply(BookSpreadData data)
    {
        if (fullSpreadImage != null)
        {
            fullSpreadImage.sprite = data.imageA;
            fullSpreadImage.enabled = data.imageA != null;
        }

        if (noteTopLeft != null)
            noteTopLeft.text = data.textA;

        if (noteTopRight != null)
            noteTopRight.text = data.textB;

        if (noteBottom != null)
            noteBottom.text = data.textC;
    }
}