using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Item", menuName = "Item/Recipe Item")]
public class RecipeItem : Item
{
    public Recipe TargetRecipe;

    public override void OnAfterDeserialize() {
        base.OnAfterDeserialize();
        UseCommand = new UseRecipeCommand(TargetRecipe);
    }
}
