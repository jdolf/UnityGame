using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICategory : MonoBehaviour
{
    public List<UICategoryItem> UICategoryItems = new List<UICategoryItem>();
    public UICategoryListener Listener;
    UICategoryItem Selection;

    void Awake() {
        UICategoryItems.ForEach(x => x.RegisterParent(this));
    }

    void Start() {
        SelectCategoryItem(UICategoryItems[0]);
    }

    public void SelectCategoryItem(UICategoryItem uiCategoryItem) {
        ResetSelections();
        Selection = uiCategoryItem;
        uiCategoryItem.SetSelected(true);
        Listener.CategorySelected(uiCategoryItem.FilterValue);
    }

    public void ResetSelections() {
        UICategoryItems.ForEach(x => x.SetSelected(false));
    }

    public void RegisterListener(UICategoryListener listener) {
        Listener = listener;
    }
}
