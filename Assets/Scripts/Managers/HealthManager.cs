using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, StatsChangedListener
{
    public PlayerStatsManager PlayerStats;
    public UIHealthBar UIBar;
    protected HealthListener listener;
    public int HP { get; set; } = 50;
    public int MaxHP { get; set; }

    void Awake()
    {
        PlayerStats.AddListener(this);
        listener = UIBar;
    }

    public void StatsChanged()
    {
        MaxHP = PlayerStats.Stats.MaxHealth;
        InformListeners();
    }

    public void RemoveHealth(int hp)
    {
        this.HP -= hp;

        if (this.HP < 0) {
            this.HP = 0;
        }

        InformListeners();
    }

    public void AddHealth(int hp)
    {
        this.HP += hp;

        if (this.HP > this.MaxHP) {
            this.HP = this.MaxHP;
        }

        InformListeners();
    }

    private void InformListeners()
    {
        listener.HPChanged(this.HP, this.MaxHP);
    }

}
