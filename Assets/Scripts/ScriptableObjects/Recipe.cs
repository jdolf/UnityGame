using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : IdentifiableScriptableObject, SavableGameData, ISerializationCallbackReceiver
{
    public Item TargetItem;
    public int TargetAmount = 1;
    // Initial value
    public bool NeedsToBeUnlocked = false;
    [NonSerialized]
    public bool Unlocked = false;
    public List<Material> Materials = new List<Material>();
    public FilterClassification Filter = FilterClassification.None;
    public static List<Recipe> AllRecipes = new List<Recipe>();

    public static void LoadAllRecipes() {
        AllRecipes = new List<Recipe>(Resources.LoadAll<Recipe>("ScriptableObjects/Recipes"));
    }

    public void PopulateGameData(GameData gameData)
    {
        if (NeedsToBeUnlocked) {
            RecipeData existingData = gameData.UnlockedRecipes.Where(x => x.ID == ID).FirstOrDefault();

            RecipeData data = new RecipeData();
            data.ID = this.ID;

            if (existingData != null) {
                gameData.UnlockedRecipes.Remove(existingData);
            }

            gameData.UnlockedRecipes.Add(data);
        }
    }

    public void LoadFromGameData(GameData gameData)
    {
        if (NeedsToBeUnlocked) {
            RecipeData existingData = gameData.UnlockedRecipes.Where(x => x.ID == ID).FirstOrDefault();

            if (existingData != null) {
                Unlocked = true;
            }
        }
    }

    public bool TryUnlock() {
        if (Unlocked) {
            return false;
        }
        Unlocked = true;
        return true;
    }

    public void OnBeforeSerialize() {}

    public void OnAfterDeserialize()
    {
        Unlocked = !NeedsToBeUnlocked;
    }

    [System.Serializable]
    public struct Material {
        public Item Item;
        public int Amount;
    }

    public enum FilterClassification {
        None,
        Tools,
        Weapon
    }

}
