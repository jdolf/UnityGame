using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopTooltip : UIItemTooltip
{
    public static UIShopTooltip ShopInstance { get; private set; }
    private static int CoinsToDisplay = 0;
    private static bool IsSelling = false;
    public TextMeshProUGUI CoinsText;
    public RectTransform CoinsBackgroundRect;

    public static void Show(Item item, int coins, bool isSelling) {
        CoinsToDisplay = coins;
        IsSelling = isSelling;
        ShopInstance.ShowTooltip(item);
    }

    public new static void Hide() {
        ShopInstance.HideTooltip();
    }

    private void Awake() {
        ShopInstance = this;
        currentRect = transform.GetComponent<RectTransform>();
        rarityImage = RarityBackgroundRect.transform.GetComponent<Image>();
        borderImage = BackgroundBorderRect.transform.GetComponent<Image>();
        HideTooltip();
    }

    protected override void DisplayText(Item item) {
        base.DisplayText(item);

        if (IsSelling) {
            this.CoinsText.SetText("Sell: " + CoinsToDisplay.ToString());
        } else {
            this.CoinsText.SetText("Buy: " + CoinsToDisplay.ToString());
        }

        
        this.CoinsText.ForceMeshUpdate();

        // CoinsBackground
        Vector2 coinsTextSize = this.CoinsText.GetRenderedValues(false);
        Vector2 coinsPaddingSize = new Vector2(this.CoinsText.margin.x * 2, this.CoinsText.margin.y * 2);
        Vector2 coinsBackgroundTotalSize = coinsTextSize + coinsPaddingSize;

        CoinsBackgroundRect.sizeDelta = coinsBackgroundTotalSize;

        Vector2 coinsPosition = new Vector2(BodyWidth - 10, -10);
        CoinsBackgroundRect.anchoredPosition = coinsPosition;

        CoinsText.rectTransform.anchoredPosition = coinsPosition;
    }
}
