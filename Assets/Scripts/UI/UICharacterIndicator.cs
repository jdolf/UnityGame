using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterIndicator : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI IndicationText;
    public Image Indicator;
    public EquipmentManager EquipmentManager;

    public void ShowIndicator() {
        Indicator.gameObject.SetActive(true);
    }

    public void HideIndicator() {
        Indicator.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UICursorItem.ShowingIndicator) {
            EquipmentManager.TryEquipItem(UICursorItem.Instance.ItemStackToDisplay.Item);
        }
    }
}
