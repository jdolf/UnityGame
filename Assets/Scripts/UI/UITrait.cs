using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI HiddenName;
    public TextMeshProUGUI Name;
    public Image Background;
    public Trait Trait { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        string positiveEffects = Trait.PositiveEffects == null ? "" : Trait.PositiveEffects;
        string negativeEffects = Trait.NegativeEffects == null ? "" : Trait.NegativeEffects;
        UITraitTooltip.Show(
            Trait.Rarity,
            Trait.Description.Replace("\\n", "\n"),
            positiveEffects.Replace("\\n", "\n"),
            negativeEffects.Replace("\\n", "\n")
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITraitTooltip.Hide();
    }
}
