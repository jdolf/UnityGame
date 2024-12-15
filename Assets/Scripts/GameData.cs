using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int TotalTreesChopped;
    public int RegularTreeChopped;
    public int PineTreeChopped;
    public int WillowTreeChopped;
    public int TotalStonesMined;
    public int StoneMined;
    public int CoalOreMined;
    public int IronOreMined;
    public int GoldOreMined;
    public int StuffKilled;
    public CrewStatsData CrewStats;
    public List<ShopData> Shops = new List<ShopData>();
    public List<TaskData> Tasks = new List<TaskData>();
    public List<SkillData> Skills = new List<SkillData>();
    public List<RecipeData> UnlockedRecipes = new List<RecipeData>();
    public List<InventoryItemData> Inventory = new List<InventoryItemData>();

    [System.Serializable]
    public class TaskData
    {
        public string ID;
        public bool Completed;
        public int Progress;
    }

    [System.Serializable]
    public class CrewStatsData
    {
        public int Level;
        public int XP;
        public int Coins;
        public string Name;
    }

    [System.Serializable]
    public class ShopData
    {
        public string ID;
        public int Level;
        public int Points;
    }

    [System.Serializable]
    public class SkillData
    {
        public string ID;
        public int Level;
        public int TotalXP;
    }

    [System.Serializable]
    public class InventoryItemData
    {
        public string InventorySlotID;
        public string ItemID;
        public int Amount;
        public int Quality;
    }

    [System.Serializable]
    public class RecipeData
    {
        public string ID;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }

    public void IncreaseIntValue(string attribute, int value, AccessMode mode = AccessMode.SnakeCase)
    {
        FieldInfo field = GetDynamicField(attribute, mode);
        if (field != null && field.FieldType == typeof(int))
        {
            
            int prevValue = (int)field.GetValue(this);
            field.SetValue(this, prevValue + value);
            int newValue = (int)field.GetValue(this);
            GameDataManager.NotifyAttributeListeners(ToPascalCase(attribute), newValue);
        }
    }

    public void SetIntValue(string attribute, int value, AccessMode mode = AccessMode.SnakeCase)
    {
        FieldInfo field = GetDynamicField(attribute, mode);
        if (field != null && field.FieldType == typeof(int))
        {
            field.SetValue(this, value);
            GameDataManager.NotifyAttributeListeners(attribute, value);
        }
    }

    public void SetStringValue(string attribute, string value, AccessMode mode = AccessMode.SnakeCase)
    {
        FieldInfo field = GetDynamicField(attribute, mode);
        if (field != null && field.FieldType == typeof(string))
        {
            field.SetValue(this, value);
        }
    }

    public int GetIntValue(string attribute, AccessMode mode = AccessMode.SnakeCase)
    {
        int value = 0;
        FieldInfo field = GetDynamicField(attribute, mode);
        if (field != null && field.FieldType == typeof(int))
        {
            value = (int)field.GetValue(this);
        }

        return value;
    }

    private FieldInfo GetDynamicField(string attribute, AccessMode mode)
    {
        string formattedAttribute;

        if (mode == AccessMode.SnakeCase) {
            formattedAttribute = ToPascalCase(attribute);
        } else {
            formattedAttribute = attribute;
        }

        // Use reflection to set the value of the class attribute dynamically
        return GetType().GetField(formattedAttribute, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private string ToPascalCase(string snakeCase)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        string[] parts = snakeCase.Split('_');

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = textInfo.ToTitleCase(parts[i]);
        }

        return string.Join("", parts);
    }

    public interface SavableGameData
    {
        void PopulateGameData(GameData gameData);
        void LoadFromGameData(GameData gameData);
    }

    public enum AccessMode
    {
        SnakeCase,
        PascalCase
    }
}
