using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHotBarItemSlot : UIItemSlot
{
    public Image BackgroundBorder;
    public Image BackgroundBorderSelected;
    public Image Background;
    [HideInInspector]
    public bool Selected = false;
    [HideInInspector]
    public int HotBarOrder;

    public override void OnPointerClick(PointerEventData eventData) {}

    public void Initialize(int hotBarOrder) {
        HotBarOrder = hotBarOrder;
    }

    public void Select() {
        Selected = true;
        BackgroundBorderSelected.gameObject.SetActive(true);
        //BackgroundBorder.color = new Color(0f,0f,0f);
        Background.color = new Color(0.85f,0.85f,0.85f);
    }

    public void Unselect() {
        Selected = false;
        BackgroundBorderSelected.gameObject.SetActive(false);
        //BackgroundBorder.color = new Color(1f,1f,1f);
        Background.color = new Color(1f,1f,1f);
    }
}
