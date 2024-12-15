using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "New Shop", menuName = "Shop")]
public class Shop : IdentifiableScriptableObject, ShopItemSlotListener, SavableGameData
{
    private static Dictionary<int, int> LevelPointsDictionary = new Dictionary<int, int>
    {
        [1] = 0,
        [2] = 1000,
        [3] = 10000,
        [4] = 100000,
        [5] = 1000000
    };

    private static Dictionary<int, int> LevelBuyDiscountDictionary = new Dictionary<int, int>
    {
        [1] = 0,
        [2] = 5,
        [3] = 10,
        [4] = 15,
        [5] = 20
    };

    private static Dictionary<int, int> LevelSellBonusDictionary = new Dictionary<int, int>
    {
        [1] = 0,
        [2] = 10,
        [3] = 20,
        [4] = 30,
        [5] = 50
    };

    public static List<Shop> AllShops;
    public string DisplayName;
    public List<Item.ItemTypeClassification> AcceptedItemTypes = new List<Item.ItemTypeClassification>();
    public List<ItemAmount> SpawnableRegularItems = new List<ItemAmount>();
    public List<ItemAmount> SpawnableLimitedItems = new List<ItemAmount>();
    [HideInInspector]
    [NonSerialized]
    public List<ItemAmount> RegularItems = new List<ItemAmount>();
    [HideInInspector]
    [NonSerialized]
    public List<ItemAmount> LimitedItems = new List<ItemAmount>();
    [HideInInspector]
    [NonSerialized]
    public int Level = 1;
    [HideInInspector]
    [NonSerialized]
    public int Points = 0;
    private int MaxLevel;
    private int NextLevel;
    [HideInInspector]
    [NonSerialized]
    public int TotalTargetPoints;
    [HideInInspector]
    [NonSerialized]
    public int TargetPoints;
    [HideInInspector]
    [NonSerialized]
    public float BuyDiscountMultiplier;
    [HideInInspector]
    [NonSerialized]
    public float SellBonusMultiplier;
    private int DisplayedLimitedItems = 3;
    private List<ShopListener> Listeners = new List<ShopListener>();

    public static void LoadShops() {
        if (AllShops == null) {
            AllShops = new List<Shop>(Resources.LoadAll<Shop>("ScriptableObjects/Shops")).ToList();
        }
    }

    void Awake() {
        this.Type = IdentificationType.Shop;
    }

    public void RegisterListener(ShopListener listener) {
        Listeners.Add(listener);
    }

    public void ClearListeners() {
        Listeners.Clear();
    }

    public void GenerateNewItems() {
        Debug.Log("Generated new shop items");
        RegularItems.Clear();
        LimitedItems.Clear();

        SpawnableRegularItems.ForEach(x => {
            Item itemCopy = Instantiate(x.Item);
            itemCopy.SetQuality(x.Quality);

            ItemAmount itemAmount = new ItemAmount
            {
                Item = itemCopy,
                Amount = x.Amount,
                Quality = x.Quality
            };
            RegularItems.Add(itemAmount);
        });

        List<ItemAmount> SpawnableLimitedItemsCopy = new List<ItemAmount>();
        SpawnableLimitedItems.ForEach(x => {
            Item itemCopy = Instantiate(x.Item);
            itemCopy.SetQuality(x.Quality);

            ItemAmount itemAmount = new ItemAmount
            {
                Item = itemCopy,
                Amount = x.Amount,
                Quality = x.Quality
            };
            SpawnableLimitedItemsCopy.Add(itemAmount);
            });

        for (int i = 0; i < DisplayedLimitedItems; i++) {
            if (SpawnableLimitedItemsCopy.Count > 0) {
                int indexToCopy = UnityEngine.Random.Range(0, SpawnableLimitedItemsCopy.Count);
                LimitedItems.Add(SpawnableLimitedItemsCopy[indexToCopy]);
                SpawnableLimitedItemsCopy.RemoveAt(indexToCopy);
            }
        }
    }

    public void IncreasePoints(int amount) {
        Points += amount;
        CheckForLevelUp();
        CalculateTargetXP();
        Listeners.ForEach(x => x.PointsChanged(TargetPoints, TotalTargetPoints, Points));
    }

    private void CalculateTargetXP() {
        if (NextLevel <= MaxLevel) {
            TotalTargetPoints = LevelPointsDictionary[NextLevel] - LevelPointsDictionary[Level];
            TargetPoints = Points - LevelPointsDictionary[Level];
        } else {
            TargetPoints = 0;
            TotalTargetPoints = 0;
        }
    }

    public void CalculateDiscountMultiplier() {
        int buyDiscount = 0;
        int sellBonus = 0;
        if (NextLevel <= MaxLevel) {
            buyDiscount = LevelBuyDiscountDictionary[Level];
            sellBonus = LevelSellBonusDictionary[Level];
            BuyDiscountMultiplier = 1 - (float) buyDiscount / 100;
            SellBonusMultiplier = 1 + (float) sellBonus / 100;
        } else {
            BuyDiscountMultiplier = 1f;
            SellBonusMultiplier = 1f;
        }

        Listeners.ForEach(x => x.MultipliersChanged(buyDiscount, sellBonus));
    }

    public void CalculateDisplayedLimitedItems() {
        DisplayedLimitedItems = 2 + Level;
    }

    private void CheckForLevelUp() {
        CalculateLevelUpParams();

        if (NextLevel <= MaxLevel && Points >= LevelPointsDictionary[NextLevel]) {
            Level++;
            Listeners.ForEach(x => x.LevelChanged(Level));
            CalculateDiscountMultiplier();
            CheckForLevelUp();
        }
    }

    private void CalculateLevelUpParams() {
        MaxLevel = LevelPointsDictionary.Keys.LastOrDefault();
        NextLevel = Level + 1;
    }

    private void FullyInformListeners() {
        Listeners.ForEach(
            x => {
                x.LevelChanged(Level);
                x.PointsChanged(TargetPoints, TotalTargetPoints, Points);
            }
        );
    }

    public int CalculateBuyPrice(int amount, Item item) {
        return (int)Math.Ceiling(item.Coins * amount * BuyDiscountMultiplier);
    }

    public int CalculateSellPrice(int amount, Item item) {
        return (int)Math.Floor(item.Coins / 2 * amount * SellBonusMultiplier);
    }

    public bool ContainsAcceptedItemType(List<Item.ItemTypeClassification> exactItemTypes) {
        bool containsAtLeastOne = false;

        exactItemTypes.ForEach(x => {
            if (AcceptedItemTypes.Contains(x)) {
                containsAtLeastOne = true;
            }
        });
        
        return containsAtLeastOne;
    }

    public void ItemsBought(int cost, int amount, Item item, bool limited)
    {
        IncreasePoints(cost);
        ItemAmount itemAmount;

        if (limited) {
            itemAmount = LimitedItems.FirstOrDefault(x => x.Item.Equal(item));
        } else {
            itemAmount = RegularItems.FirstOrDefault(x => x.Item.Equal(item));
        }

        if (itemAmount != null) {
            itemAmount.Amount -= amount;
        }
    }

    public void ItemsSold(int cost, int amount, Item item)
    {
        IncreasePoints(cost);
    }

    public void PopulateGameData(GameData gameData)
    {
        ShopData existingShopData = gameData.Shops.Where(x => x.ID == ID).FirstOrDefault();

        ShopData shopData = new ShopData();
        shopData.ID = this.ID;
        shopData.Level = this.Level;
        shopData.Points = this.Points;

        if (existingShopData != null) {
            gameData.Shops.Remove(existingShopData);
        }

        gameData.Shops.Add(shopData);
    }

    public void LoadFromGameData(GameData gameData)
    {
        ShopData existingShopData = gameData.Shops.Where(x => x.ID == ID).FirstOrDefault();

        if (existingShopData != null) {
            this.Level = existingShopData.Level;
            this.Points = existingShopData.Points;
        }

        CalculateLevelUpParams();
        CalculateTargetXP();
        CalculateDiscountMultiplier();
        CalculateDisplayedLimitedItems();
        FullyInformListeners();
    }

    [System.Serializable]
    public class ItemAmount {
        public Item Item;
        public int Amount;
        public int Quality = 0;
    }
}
