using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseRecipeCommand : Command
{
    private Recipe Recipe;

    public UseRecipeCommand(Recipe recipe) {
        Recipe = recipe;
    }

    public bool Execute()
    {
        return Recipe.TryUnlock();
    }
}
