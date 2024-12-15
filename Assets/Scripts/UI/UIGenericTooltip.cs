using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGenericTooltip : MonoBehaviour
{
    public static UIGenericTooltip Instance { get; private set; }
    public RectTransform CanvasTransform;
    public TextMeshProUGUI Text;
    public RectTransform BackgroundRect;
    public RectTransform BackgroundBorderRect;
    protected RectTransform currentRect;
    protected Image borderImage;

    public static void Show(string text, int bodyWidth = 300) {
        Instance.ShowTooltip(text, false, bodyWidth);
    }

    public static void Show(string text, bool dynamicWidth = false) {
        Instance.ShowTooltip(text, dynamicWidth);
    }

    public static void Hide() {
        Instance.HideTooltip();
    }

    private void Awake() {
        Instance = this;
        currentRect = transform.GetComponent<RectTransform>();
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

    protected void DisplayText(string text, bool dynamicWidth, int bodyWidth) {
        this.Text.SetText(text);
        this.Text.ForceMeshUpdate();
        int effectsMargin = 0;

        // Background
        Vector2 textSize = this.Text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(this.Text.margin.x * 2, this.Text.margin.y * 2 + effectsMargin);

        Vector2 backgroundSize;

        if (dynamicWidth) {
            backgroundSize = new Vector2(textSize.x + paddingSize.x, textSize.y + paddingSize.y);
        } else {
            backgroundSize = new Vector2(bodyWidth, textSize.y + paddingSize.y);
        }

        BackgroundRect.sizeDelta = backgroundSize;
        BackgroundBorderRect.sizeDelta = backgroundSize;
        this.Text.rectTransform.anchoredPosition = new Vector2(0, textSize.y + paddingSize.y);
    }

    protected void ShowTooltip(string text, bool dynamicWidth, int bodyWidth = 0) {
        Update();
        gameObject.SetActive(true);
        DisplayText(text, dynamicWidth, bodyWidth);
    }

    protected void HideTooltip() {
        Text.SetText("");
        gameObject.SetActive(false);
    }
}
