using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipmentItem Item;
    public Animator Animator;

    // Start is called before the first frame update
    public void Instantiate(EquipmentItem item, PlayerMovementManager playerMovement) {
        this.Item = item;
        Animator.runtimeAnimatorController = item.AnimationController;
        Vector2 facingDirection = PlayerMovementManager.DirectionDictionary[playerMovement.facing];
        Animator.SetFloat("Facing_Horizontal", facingDirection.x);
        Animator.SetFloat("Facing_Vertical", facingDirection.y);
    }
}
