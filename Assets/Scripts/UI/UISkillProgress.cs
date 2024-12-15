using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillProgress : MonoBehaviour, SkillListener
{
    public UIBar UIBar;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ProwessText;
    public Sprite DefaultSprite;
    public Image SkillImage;
    public UIXPDrop XPDropPrefab;
    public Transform XPDropParent;

    public void Initialize(int progress, int targetProgress) {
        XPChanged(progress, targetProgress, 0, null);
    }

    public void LevelChanged(int lvl) {
        ShowLevel(lvl);
    }

    public void ProwessChanged(Prowess prowess) {
        ShowProwess(prowess);
    }

    public void XPChanged(int currentXp, int targetXp, int xpAdded, Skill lastUpdatedSkill)
    {
        UIBar.ProgressChanged(currentXp, targetXp);

        if (lastUpdatedSkill != null) {
            SkillImage.sprite = lastUpdatedSkill.SkillIcon;
        } else {
            SkillImage.sprite = DefaultSprite;
        }
        
        if (xpAdded > 0) {
            this.gameObject.SetActive(true);
            // XP Drop
            UIXPDrop xpDrop = Instantiate(XPDropPrefab, Vector3.zero, Quaternion.identity).GetComponent<UIXPDrop>();
            xpDrop.transform.SetParent(XPDropParent);
            xpDrop.transform.localPosition = Vector3.zero;
            xpDrop.Initialize(xpAdded);
        }


        if (lastUpdatedSkill != null) {
            ShowProwess(lastUpdatedSkill.Prowess);
            ShowLevel(lastUpdatedSkill.Level);
        }
        
    }

    private void ShowProwess(Prowess prowess) {
        ProwessText.text = prowess.Name;
        ProwessText.color = prowess.Color;
    }

    private void ShowLevel(int lvl) {
        LevelText.text = "lvl " + lvl;
    }
}
