using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHealthBar : MonoBehaviour, HealthListener, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform ProgressBar;
    private int lastKnownHP = 0;
    private int lastKnownMaxHP = 0;
    bool tooltipActive = false;

    public void HPChanged(int HP, int maxHP)
    {
        lastKnownHP = HP;
        lastKnownMaxHP = maxHP;
        ProgressBar.sizeDelta = new Vector2(ProgressBar.sizeDelta.x, (float) HP / (float) maxHP * 245);
        ResetToolTip();
    }

    private void ResetToolTip() {
        if (tooltipActive) {
            UIGenericTooltip.Hide();
            ShowToolTip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipActive = true;
        ShowToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipActive = false;
        UIGenericTooltip.Hide();
    }

    private void ShowToolTip() {
        UIGenericTooltip.Show(
            "Health: " + lastKnownHP + "/" + lastKnownMaxHP,
            200
        );
    }
}
