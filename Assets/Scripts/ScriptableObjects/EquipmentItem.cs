using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Item/Equipment Item")]
public class EquipmentItem : Item, ISerializationCallbackReceiver
{
    public RuntimeAnimatorController AnimationController;
    public PlayerStats AffectedStats;

    public override void OnAfterDeserialize() {
        base.OnAfterDeserialize();
        EvaluateDisplayStats();
    }

    protected void EvaluateDisplayStats() {
        this.Description = "";

        if (AffectedStats != null) {
            string neutralStats = AffectedStats.GenerateNeutralStats(this.GetType());
            string positiveEffects = AffectedStats.GeneratePositiveStats(this.GetType());
            string negativeEffects = AffectedStats.GenerateNegativeStats(this.GetType());

            if (!this.Description.Contains(neutralStats)) {
                this.Description = this.InitialDescription + "\n\n" + AffectedStats.GenerateNeutralStats(this.GetType());
            }

            if (!this.PositiveEffects.Contains(positiveEffects)) {
                this.PositiveEffects = AffectedStats.GeneratePositiveStats(this.GetType());
            }

            if (!this.NegativeEffects.Contains(negativeEffects)) {
                this.NegativeEffects = AffectedStats.GenerateNegativeStats(this.GetType());
            }
        }
    }

    public override void SetQuality(int quality) {
        base.SetQuality(quality);

        float factor = 1f;
        if (quality == 1) factor = 1.2f;
        if (quality == 2) factor = 1.35f;
        if (quality == 3) factor = 1.5f;
        if (quality == 4) factor = 1.75f;

        if (factor > 1f) {
            AffectedStats = PlayerStats.MultiplyByFactor(AffectedStats, factor);
            EvaluateDisplayStats();
        } 

    }
}
