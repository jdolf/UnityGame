public interface ShopItemSlotListener
{
    public void ItemsBought(int cost, int amount, Item item, bool limited);
    public void ItemsSold(int cost, int amount, Item item);
}
