using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Item/Weapon Item")]
public class WeaponItem : EquipmentItem, ISerializationCallbackReceiver
{
    public AttackType AttackOne = AttackType.None;
    public AttackType AttackTwo = AttackType.None;
    public Vector2[] HitboxPoints;
    public bool CanBreakRocks = false;
    public bool CanBreakTrees = false;

    public override void OnAfterDeserialize() {
        base.OnAfterDeserialize();
        this.RoughItemType = ItemTypeClassification.Weapon;
    }

    public enum AttackType {
        None,
        Swing,
        Crush,
        Stab
    }
}