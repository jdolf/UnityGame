using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemSlot : ItemSlot
{
    protected List<ShopItemSlotListener> ShopItemSlotListener = new List<ShopItemSlotListener>();
    protected Shop Shop;
    protected bool Limited;

    public ShopItemSlot(Shop shop, bool limited) {
        Shop = shop;
        Limited = limited;
        OnSuccessfulPickup += (amount, item) => Buy(amount, item);
    }

    public override void UILeftClicked() {
        if (!Empty && UICursorItem.CanAddItemAmount(1) && CrewManager.Instance.Coins >= itemstack.Item.Coins) {
            base.UIRightClicked();
        }
    }

    public override void UIRightClicked() {
        // TODO Buy all / or go into inventory
    }

    protected bool Buy(int amount, Item item) {
        int cost = Shop.CalculateBuyPrice(amount, item);;
        CrewManager.DecreaseCoins(cost);
        ShopItemSlotListener.ForEach(x => x.ItemsBought(cost, amount, item, Limited));
        return true;
    }

    public void AddShopItemSlotListener(ShopItemSlotListener listener) {
        ShopItemSlotListener.Add(listener);
    }
}
