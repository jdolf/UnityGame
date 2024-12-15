using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotBarManager : MonoBehaviour
{
    public Transform ItemsParent;
    public Transform PotionsParentParent;
    public GameObject UIHotBarItemSlotPrefab;
    private UIHotBarItemSlot SelectedSlot;

    private List<UIHotBarItemSlot> UIHotBarItemSlots = new List<UIHotBarItemSlot>();

    void Start() {
        SelectHotBarSlot(UIHotBarItemSlots[0]);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            SelectHotBarSlot(UIHotBarItemSlots[0]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            SelectHotBarSlot(UIHotBarItemSlots[1]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            SelectHotBarSlot(UIHotBarItemSlots[2]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            SelectHotBarSlot(UIHotBarItemSlots[3]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            SelectHotBarSlot(UIHotBarItemSlots[4]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6)) {
            SelectHotBarSlot(UIHotBarItemSlots[5]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            SelectHotBarSlot(UIHotBarItemSlots[6]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            SelectHotBarSlot(UIHotBarItemSlots[7]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            SelectHotBarSlot(UIHotBarItemSlots[8]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            SelectHotBarSlot(UIHotBarItemSlots[9]);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
            SelectPreviousHotBarSlot();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
            SelectNextHotBarSlot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            SelectedSlot.Execute();
        }
    }

    public void AddItemSlot(ItemSlot itemSlot) {
        UIHotBarItemSlot uiHotBarSlot = Instantiate(this.UIHotBarItemSlotPrefab, Vector3.zero, Quaternion.identity).GetComponent<UIHotBarItemSlot>();
        uiHotBarSlot.Initialize(UIHotBarItemSlots.Count);
        UIHotBarItemSlots.Add(uiHotBarSlot);
        uiHotBarSlot.transform.SetParent(ItemsParent);
        uiHotBarSlot.transform.localScale = Vector3.one;
        itemSlot.AddListener(uiHotBarSlot);
        uiHotBarSlot.AddListener(itemSlot);
    }

    private void SelectHotBarSlot(UIHotBarItemSlot uiHotBarItemSlot) {
        UIHotBarItemSlots.ForEach(x => x.Unselect());
        uiHotBarItemSlot.Select();
        SelectedSlot = uiHotBarItemSlot;
    }

    private void SelectNextHotBarSlot() {
        int nextOrder = SelectedSlot.HotBarOrder + 1;

        if (nextOrder > 9) {
            nextOrder = 0;
        }

        SelectHotBarSlot(UIHotBarItemSlots[nextOrder]);
    }

    private void SelectPreviousHotBarSlot() {
        int previousOrder = SelectedSlot.HotBarOrder - 1;

        if (previousOrder < 0) {
            previousOrder = 9;
        }

        SelectHotBarSlot(UIHotBarItemSlots[previousOrder]);
    }
}
