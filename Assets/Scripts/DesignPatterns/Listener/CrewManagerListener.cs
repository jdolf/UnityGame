public interface CrewManagerListener
{
    public void NameChanged(string name);
    public void CoinsChanged(int coins);
    public void LevelChanged(int level);
    public void XPChanged(int targetXP, int totalTargetXP, int totalXP);
}
