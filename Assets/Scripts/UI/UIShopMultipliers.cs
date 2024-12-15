using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShopMultipliers : MonoBehaviour, ShopListener
{
    public TextMeshProUGUI BuyDiscountText;
    public TextMeshProUGUI SellBonusText;

    public void LevelChanged(int level) {}

    public void MultipliersChanged(int buyDiscount, int sellBonus)
    {
        BuyDiscountText.text = "-" + buyDiscount + "%";
        SellBonusText.text = "+" + sellBonus + "%";
    }

    public void PointsChanged(int targetPoints, int totalTargetPoints, int totalPoints) {}
}
