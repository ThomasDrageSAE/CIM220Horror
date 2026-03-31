using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookLayout_ScatteredNotes : BookSpreadLayout
{
    [SerializeField] private TextMeshProUGUI noteA;
    [SerializeField] private TextMeshProUGUI noteB;
    [SerializeField] private TextMeshProUGUI noteC;
    [SerializeField] private TextMeshProUGUI noteD;
    [SerializeField] private Image imageA;
    [SerializeField] private Image imageB;

    public override void Apply(BookSpreadData data)
    {
        if (noteA != null) noteA.text = data.textA;
        if (noteB != null) noteB.text = data.textB;
        if (noteC != null) noteC.text = data.textC;
        if (noteD != null) noteD.text = data.textD;

        SetImage(imageA, data.imageA);
        SetImage(imageB, data.imageB);
    }

    private void SetImage(Image target, Sprite sprite)
    {
        if (target == null)
            return;

        target.sprite = sprite;
        target.enabled = sprite != null;
    }
}