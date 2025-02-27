using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;
    public Sprite inventoryEmpty;

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

    public bool IsSlotTheActiveOne(InventorySlot slot)
    {
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot activeSlot = childTransform.GetComponentInChildren<InventorySlot>();
        return (slot == activeSlot);
    }

    public void RecheckActiveSlot()
    {
        ToggleActiveHighlight(activeSlotIndexNum);
    }
    
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

    public void LoadInventoryItems(string item, string stackCount)
    {
        if (string.IsNullOrEmpty(item))
        {
            Debug.LogError("[ActiveInventory] Fehler: Item-Name ist leer!");
            return;
        }

        Debug.Log($"[LoadInventoryItems] Lade Item: {item} mit StackCount: {stackCount}");

        foreach (Transform slotTransform in transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            Transform itemTransform = slotTransform.GetChild(1);
            Image itemImage = itemTransform.GetComponent<Image>();

            if (slot != null && slot.GetWeaponInfo() == null) // NUR in einen leeren Slot einfügen
            {
                ShopItem shopItem = ShopManager.Instance.FindShopItemByName(item);
                if (shopItem != null)
                {
                    slot.SetWeaponInfo(shopItem.weaponInfo);
                    slot.setStackCount(stackCount);

                    // StackCount zu einer Zahl umwandeln
                    int count = int.Parse(stackCount);
                    if (count > 1)
                    {
                        slot.setStackCountVisible();
                    }
                    else
                    {
                        slot.setStackCountInvisible();
                    }

                    itemImage.sprite = shopItem.Icon;
                    Debug.Log($"[ActiveInventory] {item} erfolgreich ins Inventar geladen!");

                    return; // Falls du Items in verschiedene Slots legen möchtest, brich hier ab
                }
                else
                {
                    Debug.LogWarning($"[ActiveInventory] Item '{item}' nicht im Shop gefunden!");
                }
            }
        }
    }




    public void ClearInventory()
    {
        int slotCount = this.transform.childCount;
        for (int i = 0; i < slotCount; i++)
        {
            Transform slotTransform = this.transform.GetChild(i);
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            GameObject item = slotTransform.Find("Item")?.gameObject;
            slot.setStackCount("1");
            slot.setStackCountInvisible();



            if (slot.GetWeaponInfo() != null)
            {
                slot.SetWeaponInfo(null);
            }

            Image itemImage = item.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = inventoryEmpty;
            }
        }

    }
}