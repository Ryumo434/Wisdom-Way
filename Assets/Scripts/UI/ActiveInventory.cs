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
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }


        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        //ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,0,0);
        //newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
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
                        Transform itemTransform = slotTransform.GetChild(1); // Zugriff auf das Image-Objekt
                        Image itemImage = itemTransform.GetComponent<Image>(); 
                        if (itemImage != null)
                        {
                            itemImage.sprite = shopItem.Icon; // Das Icon des ShopItems setzen
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


    private ShopItem FindShopItemByName(string itemName)
    {
        return ShopManager.Instance?.FindShopItemByName(itemName);
    }
}


