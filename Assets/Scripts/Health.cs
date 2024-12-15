using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class Health : MonoBehaviour, SavableGameData
{
    protected SkillManager LastHitBy;
    public int MaxHealth = 100;
    public bool IsRock = false;
    public bool IsTree = false;
    public List<SkillLevelPair> MinSkillLevelsRequired = new List<SkillLevelPair>();
    protected int Value;

    void Start() {
        this.Value = this.MaxHealth;
    }

    public void Increase(int amount) {
        this.Value += amount;
    }

    public void Decrease(int amount, SkillManager skillManager = null) {
        LastHitBy = skillManager;
        
        bool validTarget = true;

        if (skillManager != null) {
            MinSkillLevelsRequired.ForEach(x => {
            foreach (Skill skill in LastHitBy.Skills) {
                if (skill.EqualToSkill(x.Skill) && skill.Level < x.Level) {
                    validTarget = false;
                    break;
                }
            }});
        }
        
        if (validTarget) {
            this.Value -= amount;

            HitSplat.Create(amount, this.transform.position);

            if (this.Value <= 0) {
                this.Die();
            }
        }
    }

    public void Die() {
        GameDataManager.SaveJsonData(this);

        // Award XP
        if (LastHitBy != null 
            && this.gameObject.transform.TryGetComponent<EntityIdentifier>(out EntityIdentifier entityIdentifier)
            && this.gameObject.transform.TryGetComponent<XPSource>(out XPSource xpSource)) {
            IdentifiableScriptableObject identifier = entityIdentifier.Identifier;

            if (identifier.Type == IdentificationType.Tree || identifier.Type == IdentificationType.Plant) {
                LastHitBy.Farming.AddXP(xpSource.XP);
            } else if (identifier.Type == IdentificationType.Rock) {
                LastHitBy.Mining.AddXP(xpSource.XP);
            }
        }
        
        DropTable[] dropTables = this.gameObject.transform.GetComponents<DropTable>();
        
        foreach (DropTable dropTable in dropTables) {
            if (dropTable != null) {
                foreach (Item item in dropTable.GetDropItems()) {
                    DropManager.CreateDrop(item, this.transform.position);
                }
            }
        }
        
        Destroy(this.gameObject);
    }

    public void PopulateGameData(GameData gameData)
    {
        if (this.gameObject.transform.TryGetComponent<EntityIdentifier>(out EntityIdentifier entityIdentifier)) {
            IdentifiableScriptableObject identifier = entityIdentifier.Identifier;

            if (identifier.Type == IdentificationType.Tree) {
                gameData.IncreaseIntValue("total_trees_chopped", 1);
                gameData.IncreaseIntValue(identifier.ID + "_chopped", 1);
                
            } else if (identifier.Type == IdentificationType.Rock) {
                gameData.IncreaseIntValue("total_stones_mined", 1);
                gameData.IncreaseIntValue(identifier.ID + "_mined", 1);
            }
        }
    }

    public void LoadFromGameData(GameData gameData)
    {
        
    }
}
