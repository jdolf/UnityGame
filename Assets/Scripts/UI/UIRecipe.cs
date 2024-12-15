using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRecipe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Recipe Recipe;
    public CraftingScreenManager CraftingScreenManager;
    public Image Icon;
    public Animator animator;
    protected Vector3 cachedScale;
    protected bool selected = false;

    public void SetSelected(bool selected) {
        this.selected = selected;
        animator.SetBool("Selected", selected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.CraftingScreenManager.SelectRecipe(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        this.CraftingScreenManager.ShowcaseRecipe(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = cachedScale;
        this.CraftingScreenManager.UndoShowcase();
    }

    // Start is called before the first frame update
    void Start()
    {
        cachedScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
