using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITab : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private void Awake() {
        image = transform.GetComponent<Image>();
    }

    public void Activate() {
        transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void Disable() {
        transform.GetComponent<Image>().color = new Color32(147, 147, 147, 255);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManagementScreen.ShowWindowByTab(this);
    }
}
