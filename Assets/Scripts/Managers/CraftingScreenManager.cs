using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class CraftingScreenManager : MonoBehaviour, Savable
{
    public GameObject UIRecipe;
    public GameObject UIMaterial;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public Transform RecipesParent;
    public Transform MaterialsParent;
    public Button CraftButton;
    protected List<UIRecipe> uiRecipes = new List<UIRecipe>();
    protected List<Recipe> availableRecipes = new List<Recipe>();
    protected bool hasSelection = false;
    protected UIRecipe selection = null;
    protected Recipe.FilterClassification selectedCategory = Recipe.FilterClassification.None;

    private void OnEnable() {
        DetermineAvailableRecipes();
        DisplayRecipes(selectedCategory);
        if (hasSelection) {
            SelectRecipe(selection);
        }
    }

    private void OnDisable() {
        // Disable animation when window disabled
        if (hasSelection) {
            selection.SetSelected(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DetermineAvailableRecipes();
        DisplayRecipes(selectedCategory);
        SelectRecipe(uiRecipes[0]);
    }

    public void DetermineAvailableRecipes() {
        availableRecipes.Clear();

        foreach (Recipe recipe in Recipe.AllRecipes) {
            if (recipe.Unlocked) {
                availableRecipes.Add(recipe);
            }
        }
    }

    public void TryCraft() {
        if (!hasSelection) {
            Debug.Log("Not enough resources available to craft this!");
        }

        bool craftable = true;

        foreach (Recipe.Material material in selection.Recipe.Materials) {
            int amountNeeded = material.Amount;
            int amountAvailable = InventoryManager.GetAmountOfItem(material.Item);

            if (amountAvailable < amountNeeded) {
                craftable = false;
                break;
            }
        }

        if (craftable) {
            Craft();
        }
    }

    protected void Craft() {
        ItemStack itemToCreate = new ItemStack(selection.Recipe.TargetItem);
        itemToCreate.Amount = selection.Recipe.TargetAmount;
        DropManager.CreateDrops(itemToCreate);
        
        // Remove materials
        foreach (Recipe.Material material in selection.Recipe.Materials) {
            InventoryManager.TryRemoveItems(material.Item, material.Amount);
        }

        // Update Recipe Info UI
        DisplayRecipeInfo(selection.Recipe);
    }

    public void SelectRecipe(UIRecipe uiRecipe) {
        ResetSelections();
        this.hasSelection = true;
        this.selection = uiRecipe;
        uiRecipe.SetSelected(true);
        DisplayRecipeInfo(uiRecipe.Recipe);
    }

    public void ShowcaseRecipe(UIRecipe uiRecipe) {
        DisplayRecipeInfo(uiRecipe.Recipe);
    }

    public void UndoShowcase() {
        DisplayRecipeInfo(selection.Recipe);
    }

    public void ResetSelections() {
        foreach (UIRecipe uiRecipe in uiRecipes) {
            uiRecipe.SetSelected(false);
        }
    }

    public void RegisterUIRecipe(UIRecipe recipe) {
        this.uiRecipes.Add(recipe);
    }

    protected void DisplayRecipes(Recipe.FilterClassification selectedCategory) {
        // TODO Implement Filter
        uiRecipes.Clear();
        foreach (Transform child in RecipesParent) {
            Destroy(child.gameObject);
        }

        foreach (Recipe recipe in availableRecipes) {
            UIRecipe uiRecipe = Instantiate(this.UIRecipe, Vector3.zero, Quaternion.identity).GetComponent<UIRecipe>();
            uiRecipe.Recipe = recipe;
            uiRecipe.CraftingScreenManager = this;
            uiRecipe.Icon.sprite = recipe.TargetItem.Sprite;
            uiRecipes.Add(uiRecipe);
            uiRecipe.transform.SetParent(RecipesParent);
            uiRecipe.transform.localScale = Vector3.one;
        }
    }

    protected void DisplayRecipeInfo(Recipe recipe) {
        this.Title.SetText(recipe.TargetItem.Name);
        this.Description.SetText(recipe.TargetItem.Description);

        // Destroy children
        foreach (Transform child in MaterialsParent) {
            Destroy(child.gameObject);
        }
        
        foreach (Recipe.Material material in recipe.Materials) {
            UIMaterial uiMaterial = Instantiate(this.UIMaterial, Vector3.zero, Quaternion.identity).GetComponent<UIMaterial>();
            int amountAvailable = InventoryManager.GetAmountOfItem(material.Item);
            uiMaterial.Name.SetText(material.Item.Name);
            uiMaterial.AmountNeeded.SetText(material.Amount.ToString());
            uiMaterial.AmountAvailable.SetText(amountAvailable.ToString());
            uiMaterial.Icon.sprite = material.Item.Sprite;
            uiMaterial.transform.SetParent(MaterialsParent);
            uiMaterial.transform.localScale = Vector3.one;
            uiMaterial.DetermineColor(amountAvailable, material.Amount);
        }
    }

    public void Save(GameData gameData)
    {
        availableRecipes.ForEach(x => x.PopulateGameData(gameData));
    }

    public void Load(GameData gameData)
    {
        Recipe.AllRecipes.ForEach(x => x.LoadFromGameData(gameData));
    }
}
