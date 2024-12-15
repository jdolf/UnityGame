public interface ShopListener
{
    public void LevelChanged(int level);
    public void PointsChanged(int targetPoints, int totalTargetPoints, int totalPoints);
    public void MultipliersChanged(int buyDiscount, int sellBonus);
}
