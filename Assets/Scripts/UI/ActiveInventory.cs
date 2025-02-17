using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = null;

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        weaponToSpawn = weaponInfo.weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }

    // +++ NEU: Prüfen, ob ein Slot der aktuell aktive Slot ist
    public bool IsSlotTheActiveOne(InventorySlot slot)
    {
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot activeSlot = childTransform.GetComponentInChildren<InventorySlot>();
        return (slot == activeSlot);
    }

    // +++ NEU: Erneutes Aktualisieren des gerade aktiven Slots
    public void RecheckActiveSlot()
    {
        // Ruft erneut ToggleActiveHighlight() auf, was wieder ChangeActiveWeapon() auslöst
        ToggleActiveHighlight(activeSlotIndexNum);
    }

    // -----------------------------------------------------------
    // Diese Methoden sind unverändert und stammen aus deinem Code:
    // -----------------------------------------------------------
    public List<string> GetInventoryItems()
    {
        List<string> items = new List<string>();
        foreach (Transform slotTransform in transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.GetWeaponInfo() != null)
            {
                items.Add(slot.GetWeaponInfo().name);
            }
        }
        return items;
    }

    public void LoadInventoryItems(List<string> items)
    {
        foreach (Transform slotTransform in transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && items.Count > 0)
            {
                string itemName = items[0];
                items.RemoveAt(0);
                ShopItem shopItem = ShopManager.Instance.FindShopItemByName(itemName);

                if (shopItem != null)
                {
                    slot.SetWeaponInfo(shopItem.weaponInfo);

                    if (slotTransform.childCount > 1)
                    {
                        Transform itemTransform = slotTransform.GetChild(1);
                        Image itemImage = itemTransform.GetComponent<Image>();
                        if (itemImage != null)
                        {
                            itemImage.sprite = shopItem.Icon;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"[ActiveInventory] Item '{itemName}' nicht gefunden!");
                }
            }
        }
    }
}