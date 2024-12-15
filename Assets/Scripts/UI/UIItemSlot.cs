using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour, ItemSlotListener, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image Image;
    public TextMeshProUGUI Amount;
    public Transform QualityParent;
    public GameObject StarPrefab;
    public ItemStack itemStack;

    protected List<UIItemSlotListener> listeners = new List<UIItemSlotListener>();

    public virtual void ItemStackChanged(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        ShowNonEmpty();
    }

    public virtual void ItemStackRemoved()
    {
        ShowEmpty();
    }

    protected virtual void ShowNonEmpty() {
        string displayedAmount = "";

        if (itemStack.Item.MaxStackAmount > 1) {
            displayedAmount = itemStack.Amount.ToString();
        }

        Image.sprite = itemStack.Item.Sprite;
        Amount.SetText(displayedAmount);

        bool hasQuality = false;

        foreach (Transform child in QualityParent) {
            Destroy(child.gameObject);
        }

        if (itemStack.Item.Quality > 0) {
            hasQuality = true;
            for (int i = 0; i < itemStack.Item.Quality; i++) {
                Transform star = Instantiate(StarPrefab, Vector3.zero, Quaternion.identity).GetComponent<Transform>();
                star.SetParent(QualityParent);
                star.localScale = Vector3.one;
            }
        }

        QualityParent.gameObject.SetActive(hasQuality);
    }

    private void ShowEmpty() {
        this.itemStack = null;
        QualityParent.gameObject.SetActive(false);
        HideItemStack();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            foreach (UIItemSlotListener listener in listeners) {
                listener.UILeftClicked();
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            foreach (UIItemSlotListener listener in listeners) {
                listener.UIRightClicked();
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemStack != null) {
            UIItemTooltip.Show(itemStack.Item);
        }   
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        UIItemTooltip.Hide();
    }

    private void HideItemStack() {
        Image.sprite = null;
        Amount.SetText("");
    }

    public void AddListener(UIItemSlotListener listener) {
        listeners.Add(listener);
    }

    public void UpdateUI(ItemStack item_stack) {
        itemStack = item_stack;

        if (itemStack != null) {
            ShowNonEmpty();
        } else {
            ShowEmpty();
        }
    }

    public void Execute() {
        foreach (UIItemSlotListener listener in listeners) {
            listener.Execute();
        }
    }
}
