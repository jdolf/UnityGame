using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, Savable
{
    public UISkillProgress UISkillProgress;
    public UISkill UIMelee;
    public UISkill UIArchery;
    public UISkill UIMagic;
    public UISkill UIMining;
    public UISkill UIFarming;
    public UISkill UICrafting;
    public UISkill UICooking;
    public UISkill UIDoctoring;
    public UISkill UICharisma;
    public Skill Melee;
    public Skill Archery;
    public Skill Magic;
    public Skill Mining;
    public Skill Farming;
    public Skill Crafting;
    public Skill Cooking;
    public Skill Doctoring;
    public Skill Charisma;
    [HideInInspector]
    public List<Skill> Skills = new List<Skill>();
    public List<Prowess> Prowesses = new List<Prowess>();

    public void Load(GameData gameData)
    {
        Skills.ForEach(x => x.LoadFromGameData(gameData));
    }

    public void Save(GameData gameData)
    {
        Skills.ForEach(x => x.PopulateGameData(gameData));
    }

    // Start is called before the first frame update
    void Start()
    {
        Melee.Initialize(UIMelee, UISkillProgress);
        Archery.Initialize(UIArchery, UISkillProgress);
        Magic.Initialize(UIMagic, UISkillProgress);
        Mining.Initialize(UIMining, UISkillProgress);
        Farming.Initialize(UIFarming, UISkillProgress);
        Crafting.Initialize(UICrafting, UISkillProgress);
        Cooking.Initialize(UICooking, UISkillProgress);
        Doctoring.Initialize(UIDoctoring, UISkillProgress);
        Charisma.Initialize(UICharisma, UISkillProgress);

        UISkillProgress.Initialize(0, Skill.LevelsXP[0]);

        Skills.Add(Melee);
        Skills.Add(Archery);
        Skills.Add(Magic);
        Skills.Add(Mining);
        Skills.Add(Farming);
        Skills.Add(Crafting);
        Skills.Add(Cooking);
        Skills.Add(Doctoring);
        Skills.Add(Charisma);
    }

}
