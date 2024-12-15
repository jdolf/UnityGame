using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalItemSlot : ItemSlot
{
    public Item.ItemTypeClassification AllowedItemType;
    public ConditionalItemSlot(Item.ItemTypeClassification allowedItemType, string id) : base(id)
    {
        this.AllowedItemType = allowedItemType;

    }

    protected override bool IsCorrectItemType(Item.ItemTypeClassification itemType) {
        if (this.AllowedItemType == itemType) {
            return true;
        }
        return false;
    }    
}
