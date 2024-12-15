using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICategoryItem : MonoBehaviour, IPointerClickHandler
{
    private UICategory Parent;
    public string FilterValue;
    public Animator animator;
    protected bool selected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        Parent.SelectCategoryItem(this);
    }

    public void RegisterParent(UICategory parent) {
        this.Parent = parent;
    }

    public void SetSelected(bool selected) {
        this.selected = selected;
        animator.SetBool("Selected", selected);
    }
}
