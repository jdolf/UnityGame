using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class UITaskInfo : MonoBehaviour, CrewManagerListener
{
    private Dictionary<int, string> RewardDictionary = new Dictionary<int, string>
    {
        [2] = "2 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [3] = "3 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [4] = "4 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [5] = "5 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [6] = "6 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [7] = "7 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [8] = "8 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [9] = "9 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
        [10] = "10 Sigil Slots\nPossibility to upgrade ship to level 2\n+25 HP",
    };
    public TextMeshProUGUI RewardTitle;
    public TextMeshProUGUI RewardDescription;
    public TextMeshProUGUI ProgressBarText;
    public UIRank Rank;
    public RectTransform ProgressBar;

    public void LevelChanged(int level)
    {
        ChangeRewardText(level);
        Rank.ChangeRank(level);
    }

    public void XPChanged(int targetXP, int totalTargetXP, int totalXP)
    {
        ProgressBarText.text = targetXP + "/" + totalTargetXP + " XP";
        ProgressBar.sizeDelta = new Vector2((float) targetXP / (float) totalTargetXP * 550, ProgressBar.sizeDelta.y);
    }

    public void CoinsChanged(int coins) {}

    public void NameChanged(string name) {}

    private void ChangeRewardText(int level) {
        int NextLevel = CrewManager.Instance.Level + 1;

        RewardTitle.text = "Crew Level " + MiscUtils.ToRoman(CrewManager.Instance.Level + 1) + " Reward";

        if (NextLevel <= 10) {
            RewardDescription.text = RewardDictionary[CrewManager.Instance.Level + 1];
        } else {
            RewardDescription.text = "";
        }
    }
    
}
