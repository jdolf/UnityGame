using UnityEngine;

public interface SkillListener
{
    void XPChanged(int currentXp, int targetXp, int xpAdded, Skill skill);
    void LevelChanged(int lvl);
    void ProwessChanged(Prowess prowess);
}