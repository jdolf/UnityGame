using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShopPurse : MonoBehaviour, CrewManagerListener
{
    public TextMeshProUGUI CoinsText;

    public void CoinsChanged(int coins)
    {
        CoinsText.text = coins.ToString();
    }

    public void LevelChanged(int level)
    {

    }

    public void NameChanged(string name)
    {

    }

    public void XPChanged(int targetXP, int totalTargetXP, int totalXP)
    {

    }
}
