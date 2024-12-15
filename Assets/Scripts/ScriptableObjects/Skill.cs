using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;
[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : IdentifiableScriptableObject, SavableGameData
{
    public string Name;
    public Sprite SkillIcon;
    [NonSerialized]
    public static readonly int[] LevelsXP = {20,25,35,45,60,75,95,120,150,190,240,300,375,470,590,740,925,1160,1450,1815,2270,2840,3550,4440,5550,6940,8675,10845,13560,16950,21190,26490,33115,41395,51745,64685,80860,101075,126345,157935,197420,246775,308470,385590,481990,602490,753115,941395,1176745,1470935};
    [NonSerialized]
    public int Level = 1;
    [NonSerialized]
    public int TotalXP = 0;
    [NonSerialized]
    public int CurrentXP = 0;
    [NonSerialized]
    public int TargetXP = 0;
    [NonSerialized]
    public Prowess Prowess;
    [NonSerialized]
    protected List<SkillListener> listeners = new List<SkillListener>();

    public void Initialize(UISkill listenerUI, SkillListener skillProgress) {
        listeners.Add(listenerUI);
        listeners.Add(skillProgress);
        listenerUI.Initialize(0, LevelsXP[0]);
        TargetXP = LevelsXP[0];
        this.Prowess = Prowess.GetProwessByLevel(1);
        listenerUI.ProwessChanged(this.Prowess);
    }

    public void AddXP(int xp) {
        this.TotalXP += xp;
        this.CurrentXP += xp;

        TryLevelUp();
        listeners.ForEach(x => x.XPChanged(this.CurrentXP, this.TargetXP, xp, this));
    }

    protected void TryLevelUp() {
        while (this.CurrentXP >= this.TargetXP) {
            LevelUp();
        }
    }

    public void LevelUp() {
        this.CurrentXP -= this.TargetXP;
        this.TargetXP = LevelsXP[this.Level];
        this.Level += 1;
        listeners.ForEach(x => x.LevelChanged(this.Level));
        CalculateProwess();
        NotificationManager.AddNotification(new Notification("LEVEL UP", Name + " Level " + Level));
    }

    private void CalculateProwess() {
        this.Prowess = Prowess.GetProwessByLevel(this.Level);
        listeners.ForEach(x => x.ProwessChanged(this.Prowess));
    }

    private void CalculateLevelUpParamsFromLoad() {
        this.TargetXP = LevelsXP[this.Level - 1];

        this.CurrentXP = this.TotalXP;
        for (int i = 0; i < this.Level - 1; i++) {
            CurrentXP -= LevelsXP[i];
        }

        listeners.ForEach(x => x.XPChanged(CurrentXP, TargetXP, 0, this));
        listeners.ForEach(x => x.LevelChanged(this.Level));
    }

    public bool EqualToSkill(Skill skill) {
        return skill.ID == ID;
    }

    public void PopulateGameData(GameData gameData)
    {
        SkillData existingData = gameData.Skills.Where(x => x.ID == ID).FirstOrDefault();

        SkillData data = new SkillData();
        data.ID = this.ID;
        data.Level = this.Level;
        data.TotalXP = this.TotalXP;

        if (existingData != null) {
            gameData.Skills.Remove(existingData);
        }

        gameData.Skills.Add(data);
    }

    public void LoadFromGameData(GameData gameData)
    {
        SkillData existingData = gameData.Skills.Where(x => x.ID == ID).FirstOrDefault();

        if (existingData != null) {
            this.Level = existingData.Level;
            this.TotalXP = existingData.TotalXP;
        }

        CalculateLevelUpParamsFromLoad();
        CalculateProwess();
    }
}
