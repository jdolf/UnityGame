using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;

public class CrewManager : MonoBehaviour, Savable
{
    public static CrewManager Instance { get; private set; }

    private Dictionary<int, int> LevelXPDictionary = new Dictionary<int, int>
    {
        [1] = 0,
        [2] = 25,
        [3] = 100,
        [4] = 250,
        [5] = 500,
        [6] = 1000,
        [7] = 2500,
        [8] = 5000,
        [9] = 10000,
        [10] = 100000,
    };
    public UICrewInfo UICrewInfo;
    public UITaskInfo UITaskInfo;
    public UIShopPurse UIShopPurse;

    public int Level { get; private set; } = 1;
    public int XP { get; private set; } = 0;
    public int Coins { get; private set; } = 0;
    public string Name { get; private set; } = "Name";
    private int MaxLevel;
    private int NextLevel;
    private int TotalTargetXP;
    private int TargetXP;
    private List<CrewManagerListener> Listeners = new List<CrewManagerListener>();

    public static void IncreaseXP(int amount) {
        Instance.IncrementXP(amount);
    }

    public static void IncreaseCoins(int amount) {
        Instance.IncrementCoins(amount);
    }

    public static void DecreaseCoins(int amount) {
        Instance.DecrementCoins(amount);
    }

    public static void ChangeName(string name) {
        Instance.AlterName(name);
    }

    void Awake() {
        Instance = this;
        Listeners.Add(UICrewInfo);
        Listeners.Add(UITaskInfo);
        Listeners.Add(UIShopPurse);
    }

    private void CalculateLevelUpParams() {
        MaxLevel = LevelXPDictionary.Keys.LastOrDefault();
        NextLevel = Level + 1;
    }

    private void AlterName(string name) {
        Name = name;
        Listeners.ForEach(x => x.NameChanged(name));
    }

    private void IncrementXP(int amount) {
        XP += amount;
        CheckForLevelUp();
        CalculateTargetXP();
        Listeners.ForEach(x => x.XPChanged(TargetXP, TotalTargetXP, XP));
    }

    private void CalculateTargetXP() {
        if (NextLevel <= MaxLevel) {
            TotalTargetXP = LevelXPDictionary[NextLevel] - LevelXPDictionary[Level];
            TargetXP = XP - LevelXPDictionary[Level];
        } else {
            TargetXP = 0;
            TotalTargetXP = 0;
        }
    }

    private void IncrementCoins(int amount) {
        Coins += amount;
        Listeners.ForEach(x => x.CoinsChanged(Coins));
    }

    private void DecrementCoins(int amount) {
        Coins -= amount;
        Listeners.ForEach(x => x.CoinsChanged(Coins));
    }

    private void CheckForLevelUp() {
        CalculateLevelUpParams();

        if (NextLevel <= MaxLevel && XP >= LevelXPDictionary[NextLevel]) {
            Level++;
            Listeners.ForEach(x => x.LevelChanged(Level));
            CheckForLevelUp();
        }
    }

    private void FullyInformListeners() {
        Listeners.ForEach(
            x => {
                x.LevelChanged(Level);
                x.CoinsChanged(Coins);
                x.XPChanged(TargetXP, TotalTargetXP, XP);
                x.NameChanged(Name);
            }
        );
    }

    public void Load(GameData gameData)
    {
        CrewStatsData existingCrewData = gameData.CrewStats;

        if (existingCrewData != null) {
            Debug.Log("not equals null");
            Level = gameData.CrewStats.Level;
            XP = gameData.CrewStats.XP;
            Coins = gameData.CrewStats.Coins;
            Name = gameData.CrewStats.Name;
        }

        CalculateLevelUpParams();
        CalculateTargetXP();
        FullyInformListeners();
    }

    public void Save(GameData gameData)
    {
        gameData.CrewStats = new CrewStatsData
        {
            Level = Level,
            XP = XP,
            Coins = Coins,
            Name = Name
        };

        Debug.Log("saving und rofling");
    }
}
