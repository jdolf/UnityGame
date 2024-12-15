using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIShopInventorySlot : UIItemSlot
{
    public Shop Shop;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemStack != null && Shop != null) {
            bool containsItemType = Shop.ContainsAcceptedItemType(itemStack.Item.ExactItemTypes);
            if (containsItemType) {
                UIShopTooltip.Show(itemStack.Item, Shop.CalculateSellPrice(1, itemStack.Item), true);
            } else {
                UIGenericTooltip.Show("Shop won't buy.", true);
            }
            
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        UIShopTooltip.Hide();
        UIGenericTooltip.Hide();
    }

    public void Initialize(Shop shop) {
        Shop = shop;

        if (itemStack != null && Shop != null) {
            if (!Shop.ContainsAcceptedItemType(itemStack.Item.ExactItemTypes)) {
                Image.color = new Color(1f, 1f, 1f, 0.3f);
            } else {
                Image.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    public override void ItemStackRemoved() {
        base.ItemStackRemoved();
        UIShopTooltip.Hide();
        UIGenericTooltip.Hide();
    }
}
