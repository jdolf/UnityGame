using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rarity;

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait")]
public class Trait : IdentifiableScriptableObject
{
    public string Name;
    public string Description;
    public string AdditionalPositiveEffects = "";
    public string AdditionalNegativeEffects = "";
    [HideInInspector]
    public string PositiveEffects;
    [HideInInspector]
    public string NegativeEffects;
    public FamilyClassification Family = FamilyClassification.None;
    public Classification Rarity = Classification.Common;
    public PlayerStats AffectedStats;

    private void Awake() {
        EvaluateDisplayStats();
        this.Type = IdentificationType.Trait;
    }

    protected void EvaluateDisplayStats() {
        string positiveBuffer = "";
        string negativeBuffer = "";

        if (AffectedStats != null) {
            if (AdditionalPositiveEffects.Length > 0) {
                positiveBuffer = "\n";
            }
            if (AdditionalNegativeEffects.Length > 0) {
                negativeBuffer = "\n";
            }

            this.PositiveEffects = AdditionalPositiveEffects + positiveBuffer + AffectedStats.GeneratePositiveStats(this.GetType());
            this.NegativeEffects = AdditionalNegativeEffects + negativeBuffer + AffectedStats.GenerateNegativeStats(this.GetType());
        }
    }

    public enum FamilyClassification {
        None,
        Speed,
        Sleep,
        Mood,
        Work,
        Learning,
        Power,
        DodgeChance,
        Resistance,
        FightType,
        MorningEveningType
    }
}
