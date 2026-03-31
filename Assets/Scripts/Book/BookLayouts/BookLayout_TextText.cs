using TMPro;
using UnityEngine;

public class BookLayout_TextText : BookSpreadLayout
{
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private TextMeshProUGUI rightText;

    public override void Apply(BookSpreadData data)
    {
        if (leftText != null)
            leftText.text = data.textA;

        if (rightText != null)
            rightText.text = data.textB;
    }
}