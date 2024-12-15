using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity", menuName = "Rarity")]
public class Rarity : ScriptableObject
{
    public string Name;
    public Color32 PrimaryColor;
    public Color32 SecondaryColor;
    public int TraitWeight;
    public Classification Identifier;
    protected static List<Rarity> allRarities;
    
    public enum Classification {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
        Mythical = 5
    }

    public static Rarity GetRarityByClassification(Classification classification) {
        return allRarities.Find(element => element.Identifier == classification);
    }

    public static List<Rarity> GetAllRarities() {
        return allRarities;
    }

    public static void LoadRarities() {
        if (allRarities == null) {
            allRarities = new List<Rarity>(Resources.LoadAll<Rarity>("ScriptableObjects/Rarities")).OrderBy(e => e.Identifier).ToList();
        }
    }
}
