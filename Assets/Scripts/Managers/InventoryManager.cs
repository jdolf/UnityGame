using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, Savable
{
    public static InventoryManager Instance { get; private set; }
    public List<ItemSlot> InventorySlots = new List<ItemSlot>();
    protected List<UIItemSlot> uiInventorySlots = new List<UIItemSlot>();
    public Transform InventorySlotsParent;
    public GameObject UIInventorySlot;
    public UIHotBarManager UIHotBarManager;

    void Awake() {
        Instance = this;
        for (int i = 0; i < 10; i++) {
            UIItemSlot uiInventorySlot = Instantiate(this.UIInventorySlot, Vector3.zero, Quaternion.identity).GetComponent<UIItemSlot>();
            uiInventorySlots.Add(uiInventorySlot);
            uiInventorySlot.transform.SetParent(InventorySlotsParent);
            uiInventorySlot.transform.localScale = Vector3.one;

            ItemSlot itemSlot = new ItemSlot("inventory_slot_" + (i + 1));
            UIHotBarManager.AddItemSlot(itemSlot);
            
            itemSlot.AddListener(uiInventorySlot);

            uiInventorySlot.AddListener(itemSlot);
            
            InventorySlots.Add(itemSlot);
        }
    }

    public static bool TryAddItem(Item item) {
        return Instance.tryAddItem(item);
    }

    public static bool CanAddItemAmount(Item item, int amount) {
        return Instance.canAddItemAmount(item, amount);
    }

    public static void TryForceAddItem(Item item) {
        Instance.tryForceAddItem(item);
    }

    public static bool TryRemoveItem(Item item) {
        return Instance.tryRemoveItem(item);
    }

    public static bool TryRemoveItems(Item item, int amount) {
        bool allRemoved = true;
        for (int i = 0; i < amount; i++) {
            if (!Instance.tryRemoveItem(item)) {
                allRemoved = false;
            }
        }

        return allRemoved;
    }

    private bool tryRemoveItem(Item item)
    {
        bool allRemoved = false;

         foreach (ItemSlot slot in InventorySlots) {
            if (!slot.Empty && slot.Itemstack.Item == item) {
                int restToRemove = slot.TryRemoveItemAmount(item, 1);

                if (restToRemove == 0) {
                    allRemoved = true;
                    break;
                }
            }
        }

        return allRemoved;
    }

    public static int GetAmountOfItem(Item item) {
        return Instance.getAmountOfItem(item);
    }

    private bool tryAddItem(Item item) {
        bool allAdded = false;
        List<ItemSlot> sortedInventorySlots = InventorySlots.FindAll(e => !e.Empty);
        List<ItemSlot> emptyInventorySlots = InventorySlots.FindAll(e => e.Empty);
        sortedInventorySlots.AddRange(emptyInventorySlots);

        // Add to existing stacks first
        foreach (ItemSlot inventorySlot in sortedInventorySlots) {
            int restToAdd = inventorySlot.TryAddItemAmount(item, 1);

            if (restToAdd == 0) {
                allAdded = true;
                break;
            }
        }

        return allAdded;
    }

    private bool canAddItemAmount(Item item, int amount) {
        List<ItemSlot> emptyInventorySlots = InventorySlots.FindAll(e => e.Empty);

        if (emptyInventorySlots.Count > 0) {
            return true;
        }

        List<ItemSlot> nonEmptyInventorySlots = InventorySlots.FindAll(e => !e.Empty);

        foreach (ItemSlot inventorySlot in nonEmptyInventorySlots) {
            if (inventorySlot.CanAddItemAmount(item, amount)) {
                return true;
            }
        }

        return false;
    }

    private void tryForceAddItem(Item item) {
        if (!TryAddItem(item)) {
            DropManager.CreateDrop(item, null, DropManager.DropDirection.PlayerFacing, 10f);
        }
    }

    private int getAmountOfItem(Item item) {
        int amount = 0;
        foreach (ItemSlot slot in InventorySlots) {
            if (slot.Empty) {
                continue;
            }
            
            if (slot.Itemstack.Item == item) {
                amount += slot.Itemstack.Amount;
            }
        }

        return amount;
    }

    public void Save(GameData gameData)
    {
        InventorySlots.ForEach(x => x.PopulateGameData(gameData));
    }

    public void Load(GameData gameData)
    {
        InventorySlots.ForEach(x => x.LoadFromGameData(gameData));
    }
}
