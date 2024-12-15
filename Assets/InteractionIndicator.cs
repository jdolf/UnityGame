using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    public const int FlashIntervalAmount = 25;
    public static InteractionIndicator Instance;
    public TextMeshProUGUI IndicatorText;
    private int FlashInterval;
    private bool TextIsBig = false;
    //private static InteractionIndicator currentInteractionIndicator;

    void Awake() {
        Instance = this;
        Instance.gameObject.SetActive(false);
    }

    public static void Show(Vector3 playerPosition) {
        if (Instance != null) {
            Instance.gameObject.SetActive(true);
            Instance.FlashInterval = FlashIntervalAmount;
            Instance.TextIsBig = false;
            Instance.IndicatorText.text = "[SPACE]";
            Instance.transform.localPosition = playerPosition + new Vector3(0f, 180f, 0f);
        }
        
    }

    public static void Hide() {
        if (Instance != null) {
            Instance.IndicatorText.fontSize = 64;
            Instance.IndicatorText.text = "";
            Instance.FlashInterval = 0;
            Instance.TextIsBig = false;
            Instance.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {
        FlashInterval--;

        if (FlashInterval <= 0) {
            if (TextIsBig) {
                IndicatorText.fontSize = 64;
            } else {
                IndicatorText.fontSize = 84;
            }

            TextIsBig = !TextIsBig;
            Instance.FlashInterval = FlashIntervalAmount;
        }
    }
}
