using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour, StatsChangedListener
{
    public PlayerStats DefaultStats;
    [HideInInspector]
    public PlayerStats Stats;
    private EquipmentManager equipmentManager;
    private TraitManager traitManager;
    // For efficiency reasons, some classes like to get notified about changes and do their calculations beforehand
    private List<StatsChangedListener> listeners = new List<StatsChangedListener>();

    private void Awake() {
        equipmentManager = transform.GetComponent<EquipmentManager>();
        traitManager = transform.GetComponent<TraitManager>();
        //Stats = PlayerStats.Add(DefaultStats, new PlayerStats());
    }

    private void Start() {
        UpdateStats();
    }

    public void UpdateStats() {
        PlayerStats equipmentStats = equipmentManager.CalculateStats();
        PlayerStats traitStats = traitManager.CalculateStats();
        PlayerStats exteriorStats = PlayerStats.Add(equipmentStats, traitStats);

        Stats = PlayerStats.Add(exteriorStats, DefaultStats);
        NotifyListeners();
    }

    private void NotifyListeners() {
        foreach (StatsChangedListener listener in listeners) {
            listener.StatsChanged();
        }
    }

    public void AddListener(StatsChangedListener listener) {
        this.listeners.Add(listener);
    }

    public void StatsChanged()
    {
        UpdateStats();
    }
}
