using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMaterial : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Slash;
    public TextMeshProUGUI AmountAvailable;
    public TextMeshProUGUI AmountNeeded;

    protected void PaintMaterialText(Color color) {
        Slash.color = color;
        AmountAvailable.color = color;
        AmountNeeded.color = color;
    }

    public void DetermineColor(int amountAvailable, int amountNeeded) {
        if (amountAvailable >= amountNeeded) {
            PaintMaterialText(new Color32(0,180,0,255));
        } else {
            PaintMaterialText(new Color32(200,0,0,255));
        }
    }

}
