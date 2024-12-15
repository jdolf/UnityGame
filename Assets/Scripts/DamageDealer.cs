using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public SkillManager SkillManager;
    public PlayerStatsManager PlayerStats;
    public Weapon Weapon;

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.GetComponent<Health>();

        if (health != null) {
            bool breakable = true;

            if (health.IsRock && health.IsRock != Weapon.Item.CanBreakRocks) {
                breakable = false;
            }

            if (health.IsTree && health.IsTree != Weapon.Item.CanBreakTrees) {
                breakable = false;
            }

            if (health != null && breakable) {
                int damageMin = PlayerStats.Stats.DamageMin;
                int damageMax = PlayerStats.Stats.DamageMax;
                health.Decrease(Random.Range(damageMin, damageMax + 1), SkillManager);
            }
        }
    }
    
}
