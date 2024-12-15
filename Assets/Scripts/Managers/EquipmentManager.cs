using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, ItemSlotListener, Savable
{
    public UIConditionalItemSlot UIHelmetSlot;
    public UIConditionalItemSlot UIChestSlot;
    public UIConditionalItemSlot UILegSlot;
    public UIConditionalItemSlot UIWeaponSlot;
    public UIConditionalItemSlot UIShieldSlot;
    private PlayerStatsManager statsManager;
    private SkillManager skillManager;
    public Transform SwordSlotParent;
    public Transform HelmetSlotParent;
    public Transform ChestArmorSlotParent;
    public Transform LegArmorSlotParent;
    public Transform ShieldSlotParent;
    public GameObject WeaponPrefab;
    public GameObject EquipmentPrefab;
    public WeaponItem UnarmedWeaponItem;
    
    public InventoryManager InventoryManager;
    public List<ConditionalItemSlot> equipmentSlots = new List<ConditionalItemSlot>();
    protected List<UIConditionalItemSlot> uiEquipmentSlots = new List<UIConditionalItemSlot>();
    private PlayerMovementManager playerMovement;

    private void Awake() {
        statsManager = transform.GetComponent<PlayerStatsManager>();
        skillManager = transform.GetComponent<SkillManager>();
        playerMovement = transform.GetComponent<PlayerMovementManager>();
        uiEquipmentSlots.Add(UIHelmetSlot);
        uiEquipmentSlots.Add(UIChestSlot);
        uiEquipmentSlots.Add(UILegSlot);
        uiEquipmentSlots.Add(UIWeaponSlot);
        uiEquipmentSlots.Add(UIShieldSlot);

        int counter = 0;
        foreach (UIConditionalItemSlot uiSlot in uiEquipmentSlots) {
            counter++;
            ConditionalItemSlot newSlot = new ConditionalItemSlot(uiSlot.AllowedItemType, "equipment_slot_" + counter);
            newSlot.AddListener(uiSlot);
            newSlot.AddListener(this);
            uiSlot.AddListener(newSlot);
            equipmentSlots.Add(newSlot);
        }

        UpdateVisualEquipment();
    }

    public bool TryEquipItem(Item item) {
        foreach (ConditionalItemSlot equipmentSlot in equipmentSlots) {
            if (equipmentSlot.AllowedItemType == item.RoughItemType) {
                if (equipmentSlot.Empty) {
                    equipmentSlot.TryAddItemAmount(item, 1);
                    UICursorItem.TryRemoveItemAmount(UICursorItem.Instance.ItemStackToDisplay.Item, 1);
                } else {
                    Item previousItem = equipmentSlot.Itemstack.Item;
                    equipmentSlot.TryRemoveItemAmount(equipmentSlot.Itemstack.Item, 1);
                    equipmentSlot.TryAddItemAmount(item, 1);
                    InventoryManager.TryForceAddItem(previousItem);
                    UICursorItem.TryRemoveItemAmount(UICursorItem.Instance.ItemStackToDisplay.Item, 1);
                }
                UpdateVisualEquipment();
                CalculateStats();
                statsManager.StatsChanged();
                return true;
            }
        }
        return false;
    }

    private void UpdateVisualEquipment() {
        Debug.Log("Updating visual equipment");
        foreach (ConditionalItemSlot itemSlot in equipmentSlots) {
            Transform slotParent = null;

            switch (itemSlot.AllowedItemType) {
                case Item.ItemTypeClassification.Weapon:
                    slotParent = SwordSlotParent;
                    break;
                case Item.ItemTypeClassification.Helmet:
                    slotParent = HelmetSlotParent;
                    break;
                case Item.ItemTypeClassification.ChestArmor:
                    slotParent = ChestArmorSlotParent;
                    break;
                case Item.ItemTypeClassification.LegArmor:
                    slotParent = LegArmorSlotParent;
                    break;
                case Item.ItemTypeClassification.Shield:
                    slotParent = ShieldSlotParent;
                    break;
            }

            if (!itemSlot.Empty) {
                foreach (Transform child in slotParent) {
                    Destroy(child.gameObject);
                }
                
                if (itemSlot.Itemstack.Item.RoughItemType == Item.ItemTypeClassification.Weapon) {
                    WeaponItem weaponToEquip = (WeaponItem) itemSlot.Itemstack.Item;
                    Weapon weapon = Instantiate(WeaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<Weapon>();
                    weapon.Instantiate(weaponToEquip, statsManager, playerMovement, skillManager);
                    weapon.transform.SetParent(SwordSlotParent);
                    weapon.transform.localPosition = Vector3.zero;
                } else {
                    EquipmentItem equipmentToEquip = (EquipmentItem) itemSlot.Itemstack.Item;
                    Equipment equipment = Instantiate(EquipmentPrefab, Vector3.zero, Quaternion.identity).GetComponent<Equipment>();
                    equipment.Instantiate(equipmentToEquip, playerMovement);
                    equipment.transform.SetParent(slotParent);
                    equipment.transform.localPosition = Vector3.zero;
                }
            } else {
                foreach (Transform child in slotParent) {
                    Destroy(child.gameObject);
                }

                if (itemSlot.AllowedItemType == Item.ItemTypeClassification.Weapon) {
                    Weapon weapon = Instantiate(WeaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<Weapon>();
                    weapon.Instantiate(UnarmedWeaponItem, statsManager, playerMovement, skillManager);
                    weapon.transform.SetParent(SwordSlotParent);
                    weapon.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    public PlayerStats CalculateStats() {
        PlayerStats newStats = new PlayerStats();

        foreach (ConditionalItemSlot slot in equipmentSlots) {
            if (slot.AllowedItemType == Item.ItemTypeClassification.Weapon && slot.Empty) {
                // Use unarmed stats
                newStats = PlayerStats.Add(UnarmedWeaponItem.AffectedStats, newStats);
            }

            if (!slot.Empty) {
                EquipmentItem item = (EquipmentItem) slot.Itemstack.Item;

                newStats = PlayerStats.Add(item.AffectedStats, newStats);
            }
        }

        return newStats;
    }

    public void ItemStackChanged(ItemStack itemStack)
    {
        UpdateVisualEquipment();
        CalculateStats();
        statsManager.StatsChanged();
    }

    public void ItemStackRemoved()
    {
        UpdateVisualEquipment();
        CalculateStats();
        statsManager.StatsChanged();
    }

    public void Save(GameData gameData)
    {
        equipmentSlots.ForEach(x => x.PopulateGameData(gameData));
    }

    public void Load(GameData gameData)
    {
        equipmentSlots.ForEach(x => x.LoadFromGameData(gameData));
    }
}
