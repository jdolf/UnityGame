using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFoodBar : MonoBehaviour, FoodListener, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform ProgressBar;
    private int lastKnownFood = 0;
    private int lastKnownMaxFood = 0;
    bool tooltipActive = false;

    public void FoodChanged(int food, int maxFood)
    {
        this.lastKnownFood = food;
        this.lastKnownMaxFood = maxFood;
        ProgressBar.sizeDelta = new Vector2(ProgressBar.sizeDelta.x, (float) food / (float) maxFood * 245);
        ResetToolTip();
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

    private void ResetToolTip() {
        if (tooltipActive) {
            UIGenericTooltip.Hide();
            ShowToolTip();
        }
    }

    private void ShowToolTip() {
        UIGenericTooltip.Show(
            "Food: " + lastKnownFood + "/" + lastKnownMaxFood,
            200
        );
    }
}
