using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectLoader : MonoBehaviour
{
    public GameDataManager GameDataManager;

    void Awake()
    {
        Rarity.LoadRarities();
        Recipe.LoadAllRecipes();
        Task.LoadTasks();
        Shop.LoadShops();
        Item.LoadItems();
    }
}
