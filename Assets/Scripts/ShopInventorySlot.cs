using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventorySlot : UIItemSlotListener
{
    public Shop Shop;
    protected ItemSlot ItemSlot;
    protected bool ItemSold = false;
    protected List<ShopItemSlotListener> ShopItemSlotListener = new List<ShopItemSlotListener>();

    public ShopInventorySlot(ItemSlot itemSlot) {
        ItemSlot = itemSlot;
        itemSlot.OnSuccessfulPickup += (amount, item) => OnSuccessfulPickup(amount, item);
    }

    public void UILeftClicked() {
        if (!ItemSlot.Empty
            && Shop.ContainsAcceptedItemType(ItemSlot.Itemstack.Item.ExactItemTypes)
            && UICursorItem.Empty
            && UICursorItem.CanAddItemAmount(1)
            && CrewManager.Instance.Coins >= ItemSlot.Itemstack.Item.Coins) {
                
            ItemSold = true;
            ItemSlot.UIRightClicked();
        } else if (!UICursorItem.Empty
            && !ItemSlot.Empty
            && Shop.ContainsAcceptedItemType(ItemSlot.Itemstack.Item.ExactItemTypes)
            || !UICursorItem.Empty
            && ItemSlot.Empty) {
            ItemSlot.UILeftClicked();
        }
    }

    public void UIRightClicked() {
        // TODO Buy all / or go into inventory
    }

    protected bool OnSuccessfulPickup(int amount, Item item) {
        if (ItemSold) {
            ItemSold = false;
            UICursorItem.Hide();
            int sellValue = Shop.CalculateSellPrice(amount, item);
            CrewManager.IncreaseCoins(sellValue);
            ShopItemSlotListener.ForEach(x => x.ItemsSold(sellValue, amount, item));
            return true;
        }
        return false;
    }

    public void AddShopInventorySlotListener(ShopItemSlotListener listener) {
        ShopItemSlotListener.Add(listener);
    }

    public void ClearShopInventorySlotListeners() {
        ShopItemSlotListener.Clear();
    }

    public void Execute() {}
}
