using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;
public delegate void OnSuccessfulPickup(int amount, Item item);

public class ItemSlot : UIItemSlotListener, SavableGameData
{
    public event OnSuccessfulPickup OnSuccessfulPickup;
    private string ID;
    public bool Empty { get; private set; } = true;
    protected ItemStack itemstack;
    public ItemStack Itemstack { 
        get { return itemstack; }
        set
        {
            itemstack = value;

            if (value == null) {
                Empty = true;
                AlertListenersItemStackRemoved();
            } else {
                Empty = false;
                AlertListenersItemStackChanged(itemstack);
            }
        }
    }
    
    protected List<ItemSlotListener> listeners = new List<ItemSlotListener>();

    public ItemSlot(string id = "") {
        ID = id;
    }

    protected virtual bool IsCorrectItemType(Item.ItemTypeClassification itemType) {
        return true;
    }

    public bool TryUseItem() {
        if (!Empty) {
            if (itemstack.Item.TryUse()) {
                Debug.Log("success");
                TryRemoveItemAmount(itemstack.Item, 1);
                return true;
            }
        }

        return false;
    }

    public int TryAddItemAmount(Item item, int amount) {
        if (IsCorrectItemType(item.RoughItemType)) {
            int restToAdd = 0;
            if (Empty) {
                Itemstack = new ItemStack(item);
            }
            restToAdd = this.Itemstack.TryAddItemAmount(item, amount);
            AlertListenersItemStackChanged(itemstack);
            
            return restToAdd;
        }
        return amount;
    }

    public bool CanAddItemAmount(Item item, int amount) {
        if (Empty) {
            return true;
        } else {
            return itemstack.CanAddItemAmount(item, amount);
        }
    }

    public int TryRemoveItemAmount(Item item, int amount) {
        int restToRemove = this.Itemstack.TryRemoveItemAmount(item, amount);

        if (itemstack.IsEmpty()) {
            this.Itemstack = null;
        } else {
            AlertListenersItemStackChanged(itemstack);
        }
        
        return restToRemove;
    }

    public void AddListener(ItemSlotListener listener) {
        this.listeners.Add(listener);
    }

    public void AlertListenersItemStackChanged(ItemStack itemst) {
        foreach (ItemSlotListener listener in listeners) {
            listener.ItemStackChanged(itemst);
        }
    }

    public void AlertListenersItemStackRemoved() {
        foreach (ItemSlotListener listener in listeners) {
            listener.ItemStackRemoved();
        }
    }

    public virtual void UILeftClicked()
    {
        // If no item on cursor
        if (UICursorItem.Empty) {
            // If this ItemSlot has item
            if (!this.Empty) {
                // Pickup
                ItemStack copiedItemStack = new ItemStack(itemstack.Item);
                copiedItemStack.TryAddItemAmount(itemstack.Item, itemstack.Amount);
                UICursorItem.SetItemStackToDisplay(copiedItemStack);
                Itemstack = null;
            }
        // If item on cursor
        } else {
            int rest = 0;
            bool equalItem = true;
            // If this ItemSlot has no item
            if (this.Empty) {
                // Insert
                rest = TryAddItemAmount(UICursorItem.Instance.ItemStackToDisplay.Item, UICursorItem.Instance.ItemStackToDisplay.Amount);
            // If this ItemSlot has item
            } else {
                // Insert if possible
                if (Itemstack.Item.Equal(UICursorItem.Instance.ItemStackToDisplay.Item)) {
                    rest = TryAddItemAmount(UICursorItem.Instance.ItemStackToDisplay.Item, UICursorItem.Instance.ItemStackToDisplay.Amount);
                } else {
                    // Switch items if not equal
                    equalItem = false;
                    ItemStack previousSlotItemstack = new ItemStack(itemstack.Item);
                    previousSlotItemstack.TryAddItemAmount(itemstack.Item, itemstack.Amount);

                    ItemStack previousCursorItemstack = new ItemStack(UICursorItem.Instance.ItemStackToDisplay.Item);
                    previousCursorItemstack.TryAddItemAmount(UICursorItem.Instance.ItemStackToDisplay.Item, UICursorItem.Instance.ItemStackToDisplay.Amount);

                    Itemstack = previousCursorItemstack;
                    UICursorItem.SetItemStackToDisplay(previousSlotItemstack);
                }
            }

            if (equalItem && rest == 0) {
                UICursorItem.Hide();
            }

            if (equalItem && rest > 0 && !Empty) {
                ItemStack newItemstack = new ItemStack(itemstack.Item);
                newItemstack.TryAddItemAmount(itemstack.Item, rest);
                UICursorItem.SetItemStackToDisplay(newItemstack);
            }
        }
    }

    public virtual void UIRightClicked()
    {
        // If no item on cursor
        if (UICursorItem.Empty) {
            // If this ItemSlot has item
            if (!this.Empty) {
                Item slotItem = Itemstack.Item;
                int rest = TryRemoveItemAmount(Itemstack.Item, 1);

                if (rest == 0) {
                    ItemStack newItemstack = new ItemStack(slotItem);
                    newItemstack.TryAddItemAmount(slotItem, 1);
                    UICursorItem.SetItemStackToDisplay(newItemstack);
                    OnSuccessfulPickup(1, slotItem);
                }   
            }
        // If item on cursor
        } else {
            // If this ItemSlot has item
            if (!this.Empty) {
                Item slotItem = Itemstack.Item;
                int rest = TryRemoveItemAmount(UICursorItem.Instance.ItemStackToDisplay.Item, 1);
                Debug.Log(rest);

                if (rest == 0) {
                    UICursorItem.TryAddItemAmount(slotItem, 1);
                    OnSuccessfulPickup(1, slotItem);
                }   
            }
        }
    }

    public void PopulateGameData(GameData gameData)
    {
        InventoryItemData existingData = gameData.Inventory.Where(x => x.InventorySlotID == ID).FirstOrDefault();

        InventoryItemData data = new InventoryItemData();
        data.InventorySlotID = this.ID;

        if (!Empty) {
            data.ItemID = this.itemstack.Item.ID;
            data.Amount = this.itemstack.Amount;
            data.Quality = this.itemstack.Item.Quality;
        } else {
            data.ItemID = "";
            data.Amount = 0;
            data.Quality = 0;
        }

        if (existingData != null) {
            gameData.Inventory.Remove(existingData);
        }

        gameData.Inventory.Add(data);
    }

    public void LoadFromGameData(GameData gameData)
    {
        InventoryItemData existingData = gameData.Inventory.Where(x => x.InventorySlotID == ID).FirstOrDefault();

        if (existingData != null && existingData.ItemID != "") {
            Debug.Log("item should be loaded");
            Item item = Item.GenerateItemFromId(existingData.ItemID);
            Debug.Log(item);
            item.SetQuality(existingData.Quality);
            ItemStack itemStack = new ItemStack(item);
            itemStack.Amount = existingData.Amount;
            this.Itemstack = itemStack;
        }
    }

    public void Execute()
    {
        TryUseItem();
    }
}
