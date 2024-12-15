using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    private static List<string> InversedMultiplyFields = new List<string>
    {
        "Startup",
        "Duration",
        "Cooldown"
    };

    private static List<string> WeakenedMultiplyFields = new List<string>
    {
        "Startup",
        "Duration",
        "Cooldown"
    };

    public int DamageMin = 0;
    public int DamageMax = 0;
    public int Startup = 0;
    public int Duration = 0;
    public int Cooldown = 0;
    public int Knockback = 0;
    public int MeleeDefense = 0;
    public int ArcheryDefense = 0;
    public int MagicDefense = 0;
    public int MaxHealth = 0;

    // In %
    public int WalkSpeed = 0;
    public int XPGain = 0;
    public int DamageResistance = 0;
    public int DamagePercentage = 0;
    public int DodgeChance = 0;
    public int WorkSpeed = 0;
    

    public static PlayerStats Add(PlayerStats stats1, PlayerStats stats2) {
        PlayerStats newStats = new PlayerStats();

        foreach (FieldInfo fieldInfo in newStats.GetType().GetFields())
        {
            int value1 = (int) stats1.GetType().GetField(fieldInfo.Name).GetValue(stats1);
            int value2 = (int) stats2.GetType().GetField(fieldInfo.Name).GetValue(stats2);

            fieldInfo.SetValue(newStats, value1 + value2);
        }
        
        return newStats;
    }

    public static PlayerStats MultiplyByFactor(PlayerStats existingStats, float factor) {
        PlayerStats newStats = new PlayerStats();

        foreach (FieldInfo fieldInfo in newStats.GetType().GetFields())
        {
            int value = (int) existingStats.GetType().GetField(fieldInfo.Name).GetValue(existingStats);
            float weakeningFactor = 1f;

            if (PlayerStats.WeakenedMultiplyFields.Contains(fieldInfo.Name)) {
                weakeningFactor = 0.4f;
            }
            
            if (PlayerStats.InversedMultiplyFields.Contains(fieldInfo.Name)) {
                fieldInfo.SetValue(newStats, Mathf.RoundToInt(value * (1 - (factor - 1) * weakeningFactor)));
            } else {
                fieldInfo.SetValue(newStats, Mathf.RoundToInt(value * factor * weakeningFactor));
            }
        }
        
        return newStats;
    }

    public string GeneratePositiveStats(System.Type caller) {
        string result = "";
        if (caller != typeof(WeaponItem)) {
            result += DamageMin > 0 || DamageMax > 0 ? string.Format("+ {0} - {1} Damage\n", DamageMin, DamageMax) : "";
            result += Startup < 0 ? string.Format("-{0} Attack Startup Frames\n", Startup) : "";
            result += Duration < 0 ? string.Format("-{0} Attack Duration Frames\n", Duration) : "";
            result += Cooldown < 0 ? string.Format("-{0} Attack Cooldown Frames\n", Cooldown) : "";
        }
        if (caller != typeof(EquipmentItem)) {
            result += MeleeDefense > 0 ? string.Format("+{0} Melee Defense\n", MeleeDefense) : "";
            result += ArcheryDefense > 0 ? string.Format("+{0} Archery Defense\n", ArcheryDefense) : "";
            result += MagicDefense > 0 ? string.Format("+{0} Magic Defense\n", MagicDefense) : "";
        }

        result += Knockback > 0 ? string.Format("+{0} Knockback\n", Knockback) : "";
        result += MaxHealth > 0 ? string.Format("+{0} Max Health\n", MaxHealth) : "";
        result += WalkSpeed > 0 ? string.Format("+{0}% Walk Speed\n", WalkSpeed) : "";
        result += XPGain > 0 ? string.Format("+{0}% XP Gain\n", XPGain) : "";
        result += DamageResistance > 0 ? string.Format("+{0}% Damage Resistance\n", DamageResistance) : "";
        result += DamagePercentage > 0 ? string.Format("+{0}% Damage\n", DamagePercentage) : "";
        result += DodgeChance > 0 ? string.Format("+{0}% Dodge Chance\n", DodgeChance) : "";
        result += WorkSpeed > 0 ? string.Format("+{0}% Work Speed\n", WorkSpeed) : "";

        if (result.Length > 0) {
            result.Substring(result.Length - 2);
        }

        return result;
    }

    public string GenerateNegativeStats(System.Type caller) {
        string result = "";
        if (caller != typeof(WeaponItem)) {
            result += DamageMin < 0 || DamageMax < 0 ? string.Format("- {0} - {1} Damage\n", DamageMin, DamageMax) : "";
            result += Startup > 0 ? string.Format("+{0} Attack Startup Frames\n", Startup) : "";
            result += Duration > 0 ? string.Format("+{0} Attack Duration Frames\n", Duration) : "";
            result += Cooldown > 0 ? string.Format("+{0} Attack Cooldown Frames\n", Cooldown) : "";
        }
        if (caller != typeof(EquipmentItem)) {
            result += MeleeDefense < 0 ? string.Format("-{0} Melee Defense\n", Mathf.Abs(MeleeDefense)) : "";
            result += ArcheryDefense < 0 ? string.Format("-{0} Archery Defense\n", Mathf.Abs(ArcheryDefense)) : "";
            result += MagicDefense < 0 ? string.Format("-{0} Magic Defense\n", Mathf.Abs(MagicDefense)) : "";
        }
        
        result += Knockback < 0 ? string.Format("-{0} Knockback\n", Mathf.Abs(Knockback)) : "";
        result += MaxHealth < 0 ? string.Format("-{0} Max Health\n", Mathf.Abs(MaxHealth)) : "";
        result += WalkSpeed < 0 ? string.Format("-{0}% Walk Speed\n", Mathf.Abs(WalkSpeed)) : "";
        result += XPGain < 0 ? string.Format("-{0}% XP Gain\n", Mathf.Abs(XPGain)) : "";
        result += DamageResistance < 0 ? string.Format("-{0}% Damage Resistance\n", Mathf.Abs(DamageResistance)) : "";
        result += DamagePercentage < 0 ? string.Format("-{0}% Damage\n", Mathf.Abs(DamagePercentage)) : "";
        result += DodgeChance < 0 ? string.Format("-{0}% Dodge Chance\n", Mathf.Abs(DodgeChance)) : "";
        result += WorkSpeed < 0 ? string.Format("-{0}% Work Speed\n", Mathf.Abs(WorkSpeed)) : "";

        if (result.Length > 0) {
            result.Substring(result.Length - 2);
        }

        return result;
    }

    public string GenerateNeutralStats(System.Type caller) {
        string result = "";
        if (caller == typeof(WeaponItem)) {
            result += DamageMin > 0 || DamageMax > 0 ? string.Format("{0} - {1} Damage\n", DamageMin, DamageMax) : "";
            int attackDuration = Startup + Duration + Cooldown;
            result += Startup > 0 && Duration > 0 && Cooldown > 0 ? string.Format("{0} ( {1} | {2} | {3} ) Attack Duration\n", attackDuration, Startup, Duration, Cooldown) : "";
            Debug.Log("------------" + result);
        }
        if (caller == typeof(EquipmentItem)) {
            result += MeleeDefense > 0 ? string.Format("{0} Melee Defense\n", MeleeDefense) : "";
            result += ArcheryDefense > 0 ? string.Format("{0} Archery Defense\n", ArcheryDefense) : "";
            result += MagicDefense > 0 ? string.Format("{0} Magic Defense\n", MagicDefense) : "";
        }

        if (result.Length > 0) {
            result.Substring(result.Length - 2);
        }

        return result;
    }
}
