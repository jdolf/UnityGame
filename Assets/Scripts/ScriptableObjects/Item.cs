using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Rarity;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class Item : IdentifiableScriptableObject, ISerializationCallbackReceiver
{
    public static List<Item> AllItems;
    public string Name;
    public string InitialDescription;
    [NonSerialized]
    public string Description;
    public int InitialCoins = 0;
    [NonSerialized]
    public int Coins = 0;
    [HideInInspector]
    public string PositiveEffects;
    [HideInInspector]
    public string NegativeEffects;
    public Sprite Sprite;
    public Classification Rarity = Classification.Common;
    // Type that should be displayed on the item tooltip, also used for equipment and weapons
    public ItemTypeClassification RoughItemType = ItemTypeClassification.Material;
    // Identifier types that are used in shops
    public List<ItemTypeClassification> ExactItemTypes = new List<ItemTypeClassification>();
    public int MaxStackAmount = 1;
    [HideInInspector]
    public int Quality = 0;
    [NonSerialized]
    protected Command UseCommand;

    public static void LoadItems() {
        if (AllItems == null) {
            AllItems = new List<Item>(Resources.LoadAll<Item>("ScriptableObjects/Items")).ToList();
        }
    }

    public static Item GenerateItemFromId(string id) {
        Item item = AllItems.Where(x => x.ID == id).FirstOrDefault();

        if (item != null) {
            return Instantiate(item);
        }

        return null;
    }

    public virtual void OnAfterDeserialize()
    {
        Description = InitialDescription;
        Coins = InitialCoins;
        this.Type = IdentificationType.Item;
    }

    public enum ItemTypeClassification {
        Material = 0,
        Tool = 1,
        Weapon = 2,
        Helmet = 3,
        ChestArmor = 4,
        LegArmor = 5,
        Shield = 6,
        MiningMaterial = 7,
        MiningEquipment = 8,
        FarmingMaterial = 9,
        FarmingEquipment = 10,
        WoodcuttingMaterial = 11,
        WoodcuttingEquipment = 12,
        ForagingMaterial = 13,
        ForagingEquipment = 14,
        CombatMaterial = 15,
        CombatEquipment = 16,
        Sigil = 17,
        Recipe = 18,
    }

    public bool Equal(Item other) {
        return this.Name == other.Name && this.Quality == other.Quality;
    }

    public virtual void SetQuality(int quality) {
        this.Quality = quality;
        int extraRarity = 0;
        float extraCoinsFactor = 1f;

        if (quality == 1) {extraRarity += 1; extraCoinsFactor = 1.5f;}
        if (quality == 2) {extraRarity += 1; extraCoinsFactor = 2.5f;}
        if (quality == 3) {extraRarity += 2; extraCoinsFactor = 4f;}
        if (quality == 4) {extraRarity += 2; extraCoinsFactor = 6f;}
        
        if (quality > 0) {
            Rarity += extraRarity;
            
            Coins = Mathf.RoundToInt(Coins * extraCoinsFactor);
        }   
    }

    public bool TryUse() {
        if (UseCommand == null) {
            return false;
        }
        
        return UseCommand.Execute();
    }

    public void OnBeforeSerialize()
    {

    }
}
