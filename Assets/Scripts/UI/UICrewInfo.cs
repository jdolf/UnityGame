using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICrewInfo : MonoBehaviour, CrewManagerListener
{
    public UIRank UIRank;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Coins;

    public void CoinsChanged(int coins)
    {
        Coins.text = coins.ToString();
    }

    public void LevelChanged(int level)
    {
        UIRank.ChangeRank(level);
    }

    public void NameChanged(string name)
    {
        Name.text = name;
    }

    public void XPChanged(int targetXP, int totalTargetXP, int totalXP) {}
}
