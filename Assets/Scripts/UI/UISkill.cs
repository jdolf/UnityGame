using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkill : MonoBehaviour, SkillListener
{
    public TextMeshProUGUI Level;
    public TextMeshProUGUI XP;
    public TextMeshProUGUI Prowess;
    public RectTransform ProgressBar;
    public Skill Skill { get; set; }

    public void Initialize(int progress, int targetProgress) {
        XPChanged(progress, targetProgress, 0, null);
    }

    public void LevelChanged(int lvl)
    {
        Level.SetText("lvl " + lvl);
    }

    public void ProwessChanged(Prowess prowess)
    {
        this.Prowess.color = prowess.Color;
        this.Prowess.SetText(prowess.Name);
    }

    public void XPChanged(int currentXp, int targetXp, int xpAdded, Skill skill)
    {
        XP.SetText(currentXp + " / " + targetXp + " XP");
        ProgressBar.sizeDelta = new Vector2((float) currentXp / (float) targetXp * 350, ProgressBar.sizeDelta.y);
    }
}
