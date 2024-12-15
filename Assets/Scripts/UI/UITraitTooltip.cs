using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITraitTooltip : MonoBehaviour
{
    const int BodyWidth = 500;
    const int EffectsMargin = 10;
    public static UITraitTooltip Instance { get; private set; }
    public RectTransform CanvasTransform;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI PositiveText;
    public TextMeshProUGUI NegativeText;
    public RectTransform BackgroundRect;
    public RectTransform BackgroundBorderRect;
    public TextMeshProUGUI RarityText;
    public RectTransform RarityBackgroundRect;
    protected RectTransform currentRect;
    protected Image rarityImage;
    protected Image borderImage;

    public static void Show(Rarity.Classification rarity, string text, string positive = "", string negative = "") {
        Instance.ShowTooltip(rarity, text, positive, negative);
    }

    public static void Hide() {
        Instance.HideTooltip();
    }

    private void Awake() {
        Instance = this;
        currentRect = transform.GetComponent<RectTransform>();
        rarityImage = RarityBackgroundRect.transform.GetComponent<Image>();
        borderImage = BackgroundBorderRect.transform.GetComponent<Image>();
        HideTooltip();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / CanvasTransform.localScale.x;

        if (anchoredPosition.x + BackgroundRect.rect.width > CanvasTransform.rect.width) {
            anchoredPosition.x = CanvasTransform.rect.width - BackgroundRect.rect.width;
        }

        if (anchoredPosition.y + BackgroundRect.rect.height > CanvasTransform.rect.height) {
            anchoredPosition.y = CanvasTransform.rect.height - BackgroundRect.rect.height;
        }

        currentRect.anchoredPosition = anchoredPosition;
    }

    protected void DisplayText(Rarity.Classification rarity, string text, string positive = "", string negative = "") {
        this.Text.SetText(text);
        this.Text.ForceMeshUpdate();
        Vector2 positiveSize = new Vector2(0,0);
        Vector2 negativeSize = new Vector2(0,0);
        int effectsMargin = 0;

        if (positive.Length != 0) {
            this.PositiveText.SetText(positive);
            this.PositiveText.ForceMeshUpdate();
            positiveSize = this.PositiveText.GetRenderedValues(false);
            effectsMargin = EffectsMargin;
        }

        if (negative.Length != 0) {
            this.NegativeText.SetText(negative);
            this.NegativeText.ForceMeshUpdate();
            negativeSize = this.NegativeText.GetRenderedValues(false);
            effectsMargin = EffectsMargin;
        }

        this.RarityText.SetText(rarity.ToString() + " Trait");
        this.RarityText.ForceMeshUpdate();

        // Background
        Vector2 textSize = this.Text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(this.Text.margin.x * 2, this.Text.margin.y * 2 + effectsMargin);

        // RarityBackground
        Vector2 RarityTextSize = this.RarityText.GetRenderedValues(false);
        Vector2 rarityPaddingSize = new Vector2(this.RarityText.margin.x * 2, this.RarityText.margin.y * 2);

        Vector2 backgroundSize = new Vector2(BodyWidth, textSize.y + positiveSize.y + negativeSize.y + paddingSize.y);
        BackgroundRect.sizeDelta = backgroundSize;
        BackgroundBorderRect.sizeDelta = backgroundSize;
        RarityBackgroundRect.sizeDelta = RarityTextSize + rarityPaddingSize;
        Vector2 rarityPosition = new Vector2(10, BackgroundRect.sizeDelta.y - 10);
        RarityBackgroundRect.anchoredPosition = rarityPosition;
        RarityText.rectTransform.anchoredPosition = rarityPosition;
        PositiveText.rectTransform.anchoredPosition = new Vector2(0, negativeSize.y);
        this.Text.rectTransform.anchoredPosition = new Vector2(0, negativeSize.y + positiveSize.y + effectsMargin);

        Rarity fetchedRarity = Rarity.GetRarityByClassification(rarity);
        rarityImage.color = fetchedRarity.PrimaryColor;
        RarityText.color = fetchedRarity.SecondaryColor;
        borderImage.color = fetchedRarity.PrimaryColor;
    }

    protected void ShowTooltip(Rarity.Classification rarity, string text, string positive = "", string negative = "") {
        Update();
        gameObject.SetActive(true);
        DisplayText(rarity, text, positive, negative);
    }

    protected void HideTooltip() {
        Text.SetText("");
        PositiveText.SetText("");
        NegativeText.SetText("");
        gameObject.SetActive(false);
    }
}
