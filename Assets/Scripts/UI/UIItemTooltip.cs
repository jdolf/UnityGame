using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemTooltip : MonoBehaviour
{
    const int EffectsMargin = 10;
    const int NameDescriptionMargin = 15;
    public const int BodyWidth = 500;
    public static UIItemTooltip Instance { get; private set; }
    public RectTransform CanvasTransform;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI PositiveText;
    public TextMeshProUGUI NegativeText;
    public RectTransform BackgroundRect;
    public RectTransform BackgroundBorderRect;
    public TextMeshProUGUI RarityText;
    public RectTransform RarityBackgroundRect;
    public RectTransform QualityParent;
    public GameObject StarPrefab;
    protected RectTransform currentRect;
    protected Image rarityImage;
    protected Image borderImage;
    
    public static void Show(Item item) {
        Instance.ShowTooltip(item);
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

        if (anchoredPosition.y - BackgroundRect.rect.height < 0) {
            anchoredPosition.y = 0 + BackgroundRect.rect.height;
        }

        currentRect.anchoredPosition = anchoredPosition;
    }

    protected virtual void DisplayText(Item item) {
        this.Name.SetText(item.Name.ToUpper());
        this.Name.ForceMeshUpdate();
        Vector2 descriptionSize = new Vector2(0,0);
        Vector2 positiveSize = new Vector2(0,0);
        Vector2 negativeSize = new Vector2(0,0);
        int effectsMargin = 0;
        int nameDescriptionMargin = 0;

        if (item.Description != null && item.Description.Length != 0) {
            this.Description.SetText(item.Description);
            this.Description.ForceMeshUpdate();
            descriptionSize = this.Description.GetRenderedValues(false);
            nameDescriptionMargin = NameDescriptionMargin;
        }

        if (item.PositiveEffects.Length != 0) {
            this.PositiveText.SetText(item.PositiveEffects);
            this.PositiveText.ForceMeshUpdate();
            positiveSize = this.PositiveText.GetRenderedValues(false);
            effectsMargin = EffectsMargin;
        }

        if (item.NegativeEffects.Length != 0) {
            this.NegativeText.SetText(item.NegativeEffects);
            this.NegativeText.ForceMeshUpdate();
            negativeSize = this.NegativeText.GetRenderedValues(false);
            effectsMargin = EffectsMargin;
        }

        Vector2 qualitySize = new Vector2(0,0);
        bool hasQuality = false;
        if (item.Quality > 0) {
            hasQuality = true;
            for (int i = 0; i < item.Quality; i++) {
                Transform star = Instantiate(StarPrefab, Vector3.zero, Quaternion.identity).GetComponent<Transform>();
                star.SetParent(QualityParent);
                star.localScale = Vector3.one;
            }
            qualitySize = QualityParent.sizeDelta;
        }

        QualityParent.gameObject.SetActive(hasQuality);

        this.RarityText.SetText(item.Rarity.ToString() + " " + item.RoughItemType.ToString());
        this.RarityText.ForceMeshUpdate();

        // Background
        Vector2 textSize = this.Name.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(this.Name.margin.x * 2, this.Name.margin.y * 2 + nameDescriptionMargin + effectsMargin);
        Vector2 backgroundSize = new Vector2(BodyWidth, textSize.y + descriptionSize.y + positiveSize.y + negativeSize.y + paddingSize.y);

        // RarityBackground
        Vector2 RarityTextSize = this.RarityText.GetRenderedValues(false);
        Vector2 rarityPaddingSize = new Vector2(this.RarityText.margin.x * 2, this.RarityText.margin.y * 2);
        Vector2 rarityBackgroundTotalSize = RarityTextSize + rarityPaddingSize;
        
        BackgroundRect.sizeDelta = backgroundSize;
        BackgroundBorderRect.sizeDelta = backgroundSize;
        RarityBackgroundRect.sizeDelta = rarityBackgroundTotalSize;

        Vector2 rarityPosition = new Vector2(10, -10);
        RarityBackgroundRect.anchoredPosition = rarityPosition;
        RarityText.rectTransform.anchoredPosition = rarityPosition;
        BackgroundRect.anchoredPosition = new Vector2(0, -backgroundSize.y);
        BackgroundBorderRect.anchoredPosition = new Vector2(0, -backgroundSize.y);
        this.Name.rectTransform.anchoredPosition = new Vector2(0, 0);
        Description.rectTransform.anchoredPosition = new Vector2(0, -textSize.y - nameDescriptionMargin);
        PositiveText.rectTransform.anchoredPosition = new Vector2(0, -textSize.y - nameDescriptionMargin - descriptionSize.y - effectsMargin);
        NegativeText.rectTransform.anchoredPosition = new Vector2(0, -textSize.y - nameDescriptionMargin - descriptionSize.y - positiveSize.y - effectsMargin);
        //QualityParent.anchoredPosition = new Vector2(textSize.x + this.Name.margin.x * 2, -textSize.y / 2 - this.Name.margin.y);
        QualityParent.anchoredPosition = new Vector2(BodyWidth - 16, -45);

        Rarity fetchedRarity = Rarity.GetRarityByClassification(item.Rarity);
        rarityImage.color = fetchedRarity.PrimaryColor;
        RarityText.color = fetchedRarity.SecondaryColor;
        borderImage.color = fetchedRarity.PrimaryColor;
    }

    protected void ShowTooltip(Item item) {
        Update();
        gameObject.SetActive(true);
        DisplayText(item);
    }

    protected void HideTooltip() {
        foreach (Transform child in QualityParent) {
            Destroy(child.gameObject);
        }
        Name.SetText("");
        Description.SetText("");
        PositiveText.SetText("");
        NegativeText.SetText("");
        gameObject.SetActive(false);
    }
}
