using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICursorItem : MonoBehaviour
{
    [HideInInspector]
    public static bool Empty = true;
    [HideInInspector]
    public static bool ShowingIndicator = false;
    public static UICursorItem Instance { get; private set; }
    public List<UICharacterIndicator> UICharacterIndicators = new List<UICharacterIndicator>();
    public RectTransform CanvasTransform;
    public Image ItemImage;
    public TextMeshProUGUI Amount;
    public ItemStack ItemStackToDisplay;
    protected RectTransform currentRect;

    public static void SetItemStackToDisplay(ItemStack itemStack) {
        Instance.SetItemStack(itemStack);
    }

    public static bool CanAddItemAmount(int amount) {
        if (Empty) {
            return true;
        } else {
            return Instance.ItemStackToDisplay.AvailableSpaces >= amount;
        }
    }

    public static int TryAddItemAmount(Item other, int amount) {
        return Instance.TryAddAmount(other, amount);
    }

    public static int TryRemoveItemAmount(Item other, int amount) {
        return Instance.TryRemoveAmount(other, amount);
    }

    public static void Hide() {
        Instance.gameObject.SetActive(false);
        Instance.HideItemAmount();
    }

    private void Awake() {
        Instance = this;
        currentRect = transform.GetComponent<RectTransform>();
        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRect.anchoredPosition = Input.mousePosition  / CanvasTransform.localScale.x;
    }

    protected void SetItemStack(ItemStack itemStack) {
        Update();
        Empty = false;
        gameObject.SetActive(true);
        this.ItemStackToDisplay = itemStack;
        CheckForIndicationItems();
        UpdateItemAmount();
    }

    protected void HideItemAmount() {
        gameObject.SetActive(false);
        Empty = true;
        CheckForIndicationItems();
    }

    private void UpdateItemAmount()
    {
        this.ItemImage.sprite = ItemStackToDisplay.Item.Sprite;

        if (ItemStackToDisplay.Amount > 1) {
            this.Amount.SetText(ItemStackToDisplay.Amount.ToString());
        } else {
            this.Amount.SetText("");
        }
    }

    protected int TryRemoveAmount(Item other, int amount) {
        if (!Empty) {
            int rest = Instance.ItemStackToDisplay.TryRemoveItemAmount(other, amount);

            if (Instance.ItemStackToDisplay.IsEmpty()) {
                Hide();
            }

            CheckForIndicationItems();
            UpdateItemAmount();
            return rest;
        }
        return amount;
    }

    protected int TryAddAmount(Item other, int amount) {
        if (!Empty) {
            int rest = Instance.ItemStackToDisplay.TryAddItemAmount(other, amount);
            CheckForIndicationItems();
            UpdateItemAmount();
            return rest;
        }
        return amount;
    }

    private void CheckForIndicationItems() {
        if (Empty) {
            foreach (UICharacterIndicator ci in UICharacterIndicators) {
                ci.HideIndicator();
            }
            ShowingIndicator = false;
        }

        if (!Empty) {
            if (ItemStackToDisplay.Item.RoughItemType == Item.ItemTypeClassification.Tool || ItemStackToDisplay.Item.RoughItemType == Item.ItemTypeClassification.Weapon) {
                foreach (UICharacterIndicator ci in UICharacterIndicators) {
                    ci.ShowIndicator();
                }
                ShowingIndicator = true;
            }
        }
    }
}
