using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour, ShopListener, Savable
{
    public UIBar UIBar;
    public UIShopMultipliers UIShopMultipliers;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ShopTitleText;
    public Transform InventorySlotsParent;
    public Transform RegularShopItemsParent;
    public Transform LimitedShopItemsParent;
    public GameObject UIInventorySlot;
    public GameObject UIShopItemSlot;
    public GameObject UILimitedShopItemSlot;
    public InventoryManager InventoryManager;
    protected List<UIShopInventorySlot> UIShopInventorySlots = new List<UIShopInventorySlot>();
    protected List<UIShopItemSlot> UIShopItemSlots = new List<UIShopItemSlot>();
    protected List<ShopItemSlot> RegularShopItemSlots = new List<ShopItemSlot>();
    protected List<ShopItemSlot> LimitedShopItemSlots = new List<ShopItemSlot>();
    protected List<ShopInventorySlot> ShopInventorySlots = new List<ShopInventorySlot>();
    private Shop CurrentShop;
    public List<Shop> Shops = new List<Shop>();
    private bool InventoryCreated = false;

    public void DisplayShop(Shop shop) {
        CurrentShop = shop;
        shop.ClearListeners();
        shop.RegisterListener(this);
        shop.RegisterListener(UIShopMultipliers);

        if (!InventoryCreated) {
            InstantiateInventorySlots();
        }

        InstantiateShopSlots(shop);
        InformListenersOfNewShop(shop);
        
        // Initialize Values
        shop.CalculateDiscountMultiplier();
        shop.CalculateDisplayedLimitedItems();
        ShopTitleText.text = "- " + shop.DisplayName + " -";
        UIBar.ProgressChanged(shop.TargetPoints, shop.TotalTargetPoints);
        LevelChanged(shop.Level);

        UIManagementScreen.ShowShop();
    }

    public void GenerateNewItems() {
        Shops.ForEach(x => x.GenerateNewItems());
    }

    private void InstantiateInventorySlots() {
        /*
        UIShopInventorySlots.Clear();
        ShopInventorySlots.Clear();

        foreach (Transform child in InventorySlotsParent) {
            Destroy(child.gameObject);
        }
        */
        InventoryCreated = true;

        foreach (ItemSlot itemSlot in InventoryManager.InventorySlots) {
            UIShopInventorySlot uiInventorySlot = Instantiate(this.UIInventorySlot, Vector3.zero, Quaternion.identity).GetComponent<UIShopInventorySlot>();
            
            ShopInventorySlot shopInventorySlot = new ShopInventorySlot(itemSlot);
            ShopInventorySlots.Add(shopInventorySlot);
            UIShopInventorySlots.Add(uiInventorySlot);
            uiInventorySlot.transform.SetParent(InventorySlotsParent);
            uiInventorySlot.transform.localScale = Vector3.one;
            
            itemSlot.AddListener(uiInventorySlot);
            uiInventorySlot.AddListener(shopInventorySlot);

            uiInventorySlot.UpdateUI(itemSlot.Itemstack);
        }
    }

    private void InformListenersOfNewShop(Shop shop) {
        ShopInventorySlots.ForEach(x => {
            x.Shop = shop;
            x.ClearShopInventorySlotListeners();
            x.AddShopInventorySlotListener(shop);
        });
        UIShopInventorySlots.ForEach(x => {x.Initialize(shop);});
        UIShopItemSlots.ForEach(x => x.Shop = shop);
    }

    private void InstantiateShopSlots(Shop shop) {
        UIShopItemSlots.Clear();
        RegularShopItemSlots.Clear();
        LimitedShopItemSlots.Clear();

        foreach (Transform child in RegularShopItemsParent) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in LimitedShopItemsParent) {
            Destroy(child.gameObject);
        }

        foreach (Shop.ItemAmount ItemAmount in CurrentShop.RegularItems) {
            UIShopItemSlot uiShopItemSlot = Instantiate(this.UIShopItemSlot, Vector3.zero, Quaternion.identity).GetComponent<UIShopItemSlot>();
            ShopItemSlot shopItemSlot = InstantiateSingularShopSlot(ItemAmount, uiShopItemSlot, shop, false);
            RegularShopItemSlots.Add(shopItemSlot);
            uiShopItemSlot.transform.SetParent(RegularShopItemsParent);
            uiShopItemSlot.transform.localScale = Vector3.one;
        }

        foreach (Shop.ItemAmount ItemAmount in CurrentShop.LimitedItems) {
            UIShopItemSlot uiShopItemSlot = Instantiate(this.UILimitedShopItemSlot, Vector3.zero, Quaternion.identity).GetComponent<UIShopItemSlot>();
            ShopItemSlot shopItemSlot = InstantiateSingularShopSlot(ItemAmount, uiShopItemSlot, shop, true);
            LimitedShopItemSlots.Add(shopItemSlot);
            uiShopItemSlot.transform.SetParent(LimitedShopItemsParent);
            uiShopItemSlot.transform.localScale = Vector3.one;
        }
    }

    private ShopItemSlot InstantiateSingularShopSlot(Shop.ItemAmount itemAmount, UIShopItemSlot uiShopItemSlot, Shop shop, bool limited) {
        ShopItemSlot shopItemSlot = new ShopItemSlot(shop, limited);
        
        shopItemSlot.AddListener(uiShopItemSlot);
        shopItemSlot.AddShopItemSlotListener(shop);
        uiShopItemSlot.AddListener(shopItemSlot);

        ItemStack itemStack = new ItemStack(itemAmount.Item);
        int amount = itemAmount.Amount;

        if (amount > 0) {
            itemStack.Amount = amount;
            shopItemSlot.Itemstack = itemStack;
        }
        
        uiShopItemSlot.Initialize(shop, amount <= 0, itemStack.Item);

        UIShopItemSlots.Add(uiShopItemSlot);
        return shopItemSlot;
    }

    public void RegisterShop(Shop shop) {
        CurrentShop = shop;
    }

    public void LevelChanged(int level)
    {
        LevelText.text = "LEVEL " + level;
        UIShopItemSlots.ForEach(x => x.UpdateUI());
    }

    public void PointsChanged(int targetPoints, int totalTargetPoints, int totalPoints)
    {
        UIBar.ProgressChanged(targetPoints, totalTargetPoints);
    }

    public void Save(GameData gameData)
    {
        foreach (Shop shop in Shops) {
            shop.PopulateGameData(gameData);
        }
    }

    public void Load(GameData gameData)
    {
        Shops = Shop.AllShops;
        Debug.Log("WHATS TAKING U SO LONG-------------");
        foreach (Shop shop in Shops) {
            shop.LoadFromGameData(GameDataManager.GameData);
        }

        GenerateNewItems();
    }

    public void MultipliersChanged(int buyDiscount, int sellBonus) {}
}
