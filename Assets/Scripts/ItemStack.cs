using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    public Item Item;
    public int Amount { get; set; } = 0;
    public int AvailableSpaces { get; private set; } = 0;

    public ItemStack(Item item) {
        this.Item = item;
    }

    public int TryAddItemAmount(Item item, int amount) {
        if (this.Item.Equal(item)) {
            int maxAmount = Item.MaxStackAmount;
            int availableSpace = maxAmount - this.Amount;

            if (amount >= availableSpace) {
                int newlyUsedSpaces = availableSpace;
                int rest = amount - availableSpace;
                Amount += newlyUsedSpaces;
                AvailableSpaces = maxAmount - Amount;
                return rest;
            } else {
                int newlyUsedSpaces = amount;
                Amount += amount;
                AvailableSpaces = maxAmount - Amount;
                return 0;
            }
        }

        return amount;
    }

    public bool CanAddItemAmount(Item item, int amount) {
        if (this.Item.Equal(item)) {
            int maxAmount = Item.MaxStackAmount;
            int availableSpace = maxAmount - this.Amount;

            if (amount > availableSpace) {
                return false;
            } else {
                return true;
            }
        }

        return false;
    }

    public int TryRemoveItemAmount(Item item, int amount) {
        int maxAmount = Item.MaxStackAmount;

        if (this.Item == item) {
            if (amount > this.Amount) {
                int rest = amount - this.Amount;
                Amount = 0;
                AvailableSpaces = maxAmount - Amount;
                return rest;
            } else {
                Amount -= amount;
                AvailableSpaces = maxAmount - Amount;
                return 0;
            }
        }

        return amount;
    }

    public bool IsEmpty() {
        if (this.Amount <= 0) {
            return true;
        }
        return false;
    }
}
