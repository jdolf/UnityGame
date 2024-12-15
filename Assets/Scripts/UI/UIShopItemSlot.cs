using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIShopItemSlot : UIItemSlot
{
    public Shop Shop;
    public TextMeshProUGUI Coins;
    public GameObject SoldOutImage;

    public override void ItemStackChanged(ItemStack itemStack)
    {
        if (itemStack.Item.MaxStackAmount >= 1) {
            SoldOutImage.SetActive(false);
        }

        base.ItemStackChanged(itemStack);
    }

    public override void ItemStackRemoved() {
        SoldOutImage.SetActive(true);
        base.ItemStackRemoved();
        UIShopTooltip.Hide();
        UIGenericTooltip.Hide();
    }

    public void Initialize(Shop shop, bool soldOut, Item item) {
        Shop = shop;
        if (soldOut) {
            SoldOutImage.SetActive(true);
        }
        UpdateUI(item);
    }

    public void UpdateUI(Item fallBackItem = null) {
        if (fallBackItem != null) {
            Coins.text = Shop.CalculateBuyPrice(1, fallBackItem).ToString();
        } else if (itemStack != null) {
            Coins.text = Shop.CalculateBuyPrice(1, itemStack.Item).ToString();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemStack != null) {
            UIShopTooltip.Show(itemStack.Item, Shop.CalculateBuyPrice(1, itemStack.Item), false);
        }   
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        UIShopTooltip.Hide();
    }
}
