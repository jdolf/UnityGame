using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIXPDrop : MonoBehaviour
{
    public const int XPDropTravelHeight = 200;
    public const int DestroyDelay = 30;
    public TextMeshProUGUI XPText;
    private RectTransform rectTransform;
    private int DestroyDelayCounter = DestroyDelay;

    void Awake() {
        rectTransform = transform.GetComponent<RectTransform>();
    }

    public void Initialize(int xpAdded) {
        XPText.text = "+ " + xpAdded + " XP";
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -XPDropTravelHeight);
    }

    void FixedUpdate() {
        if (rectTransform.anchoredPosition.y < 0) {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 5);
        } else {
            DestroyDelayCounter--;
            if (DestroyDelayCounter <= 0) {
                Destroy(this.gameObject);
            }
        }
    }
}
