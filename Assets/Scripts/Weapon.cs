using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponItem Item;
    public Animator Animator;
    public DamageDealer damageDealer;
    public PolygonCollider2D HitboxCollider;

    public void Instantiate(WeaponItem item, PlayerStatsManager statsManager, PlayerMovementManager playerMovement, SkillManager skillManager) {
        this.Item = item;
        Animator.runtimeAnimatorController = item.AnimationController;
        Vector2 facingDirection = PlayerMovementManager.DirectionDictionary[playerMovement.facing];

        if (Animator.runtimeAnimatorController != null) {
            Animator.SetFloat("Facing_Horizontal", facingDirection.x);
            Animator.SetFloat("Facing_Vertical", facingDirection.y);
        }
        
        // Update Weapon layer
        if (new List<PlayerMovementManager.Facing>{PlayerMovementManager.Facing.Up, PlayerMovementManager.Facing.Left, PlayerMovementManager.Facing.UpLeft, PlayerMovementManager.Facing.DownLeft}.Contains(playerMovement.facing)) {
            this.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;
        } else {
            this.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        HitboxCollider.points = item.HitboxPoints;
        damageDealer.PlayerStats = statsManager;
        damageDealer.SkillManager = skillManager;
    }
}
