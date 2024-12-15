using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagementScreen : MonoBehaviour
{
    [HideInInspector]
    public TabWindowPair ActivePair;
    [HideInInspector]
    public bool Active = false;
    public static UIManagementScreen Instance { get; private set; }
    public TabWindowPair Inventory;
    public TabWindowPair Bio;
    public TabWindowPair Equipment;
    public TabWindowPair Crafting;
    public TabWindowPair Tasks;
    public TabWindowPair Shop;
    private List<TabWindowPair> pairs = new List<TabWindowPair>();
    public Transform Container;
    
    private void Awake() {
        Instance = this;
        pairs.Add(Inventory);
        pairs.Add(Bio);
        pairs.Add(Equipment);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            showInventory();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            showEquipment();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            showBio();
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            showTasksScreen();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            showCraftingScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            tryHideScreen();
        }
    }

    private void ToggleTab(TabWindowPair pairToDisplay) {
        HideTooltips();
        
        if (Active && ActivePair.Window == pairToDisplay.Window) {
            // Press same button on active window -> disappear
            tryHideScreen();
        } else if (Active && ActivePair.Window != pairToDisplay.Window) {
            // Press different button on active window -> disappear and show new
            Debug.Log("rofl?");
            ActivePair.Window.gameObject.SetActive(false);
            if (ActivePair.Tab != null) {
                ActivePair.Tab.Disable();
            }
            ActivePair = pairToDisplay;
            ActivePair.Window.gameObject.SetActive(true);
            if (ActivePair.Tab != null) {
                ActivePair.Tab.Activate();
                Container.gameObject.SetActive(true);
            }
        } else if (!Active) {
            // Press button on unopened window -> show
            ActivePair = pairToDisplay;
            Active = true;
            ActivePair.Window.gameObject.SetActive(true);
            if (ActivePair.Tab != null) {
                ActivePair.Tab.Activate();
                Container.gameObject.SetActive(true);
            }
            
        }
    }

    [System.Serializable]
    public class TabWindowPair {
        public Transform Window;
        public UITab Tab;
    }

    private void showInventory() {
        ToggleTab(Inventory);
    }

    private void showEquipment() {
        ToggleTab(Equipment);
    }

    private void showBio() {
        ToggleTab(Bio);
    }

    private void showCraftingScreen() {
        ToggleTab(Crafting);
    }

    private void showTasksScreen() {
        ToggleTab(Tasks);
    }

    private void showShopScreen() {
        ToggleTab(Shop);
    }

    private void tryHideScreen() {
        if (Active) {
            HideTooltips();
            ActivePair.Window.gameObject.SetActive(false);
            if (ActivePair.Tab != null) {
                ActivePair.Tab.Disable();
            }
            Container.gameObject.SetActive(false);
            ActivePair = null;
            Active = false;
        }
    }

    public static void ShowInventory() {
        Instance.showInventory();
    }

    public static void ShowEquipment() {
        Instance.showEquipment();
    }

    public static void ShowBio() {
        Instance.showBio();
    }

    public static void ShowShop() {
        Instance.showShopScreen();
    }

    public static bool IsActive() {
        return Instance.Active;
    }

    public static void ShowWindowByTab(UITab uiTab) {
        Instance.showWindowByTab(uiTab);
    }

    private void showWindowByTab(UITab uiTab) {
        foreach (TabWindowPair pair in pairs) {
            if (pair.Tab == uiTab) {
                ToggleTab(pair);
            }
        }
    }

    private void HideTooltips() {
        UITraitTooltip.Hide();
        UIItemTooltip.Hide();
        UIShopTooltip.Hide();
        UIGenericTooltip.Hide();
    }
}
