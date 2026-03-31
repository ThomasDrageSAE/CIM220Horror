using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookLayout_FlexibleMixed : BookSpreadLayout
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI textSlotA;
    [SerializeField] private TextMeshProUGUI textSlotB;
    [SerializeField] private TextMeshProUGUI textSlotC;
    [SerializeField] private TextMeshProUGUI textSlotD;

    [Header("Images")]
    [SerializeField] private Image imageSlotA;
    [SerializeField] private Image imageSlotB;
    [SerializeField] private Image imageSlotC;
    [SerializeField] private Image imageSlotD;

    public override void Apply(BookSpreadData data)
    {
        SetText(textSlotA, data.textA);
        SetText(textSlotB, data.textB);
        SetText(textSlotC, data.textC);
        SetText(textSlotD, data.textD);

        SetImage(imageSlotA, data.imageA);
        SetImage(imageSlotB, data.imageB);
        SetImage(imageSlotC, data.imageC);
        SetImage(imageSlotD, data.imageD);
    }

    private void SetText(TextMeshProUGUI target, string value)
    {
        if (target == null)
            return;

        target.text = value;
        target.gameObject.SetActive(!string.IsNullOrWhiteSpace(value));
    }

    private void SetImage(Image target, Sprite sprite)
    {
        if (target == null)
            return;

        target.sprite = sprite;
        target.gameObject.SetActive(sprite != null);
    }
}