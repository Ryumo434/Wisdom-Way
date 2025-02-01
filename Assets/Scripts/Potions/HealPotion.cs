using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;

    [SerializeField] private Sprite emptySprite; // Hier kannst du die empty.png zuweisen

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = PlayerHealth.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "healing")
            {
                playerHealth.HealPlayer(10);
                RemovePotionFromInventory();
            }
        }
    }

    private void RemovePotionFromInventory()
    {
        InventorySlot[] inventorySlots = FindObjectsOfType<InventorySlot>();

        foreach (InventorySlot slot in inventorySlots)
        {
            Transform activeChild = slot.transform.Find("Active");
            if (activeChild != null && activeChild.gameObject.activeSelf)
            {
                if (slot.GetWeaponInfo() == weaponInfo)
                {
                    if (slot.getStackCount().text != "0")
                    {
                        int newValue = int.Parse(slot.getStackCount().text) - 1;
                        slot.setStackCount(newValue.ToString());

                        if (newValue == 0)
                        {
                            slot.setStackCountInvisible();
                            continue;
                        } else
                        {
                            return;
                        }
                    } else
                    {
                        slot.setStackCountInvisible();
                    }

                    Transform itemChild = slot.transform.Find("Item");
                    if (itemChild != null)
                    {
                        Image itemImage = itemChild.GetComponent<Image>();
                        if (itemImage != null && emptySprite != null)
                        {
                            slot.RemoveWeaponInfo();
                            itemImage.sprite = emptySprite;
                            Destroy(gameObject);
                        }
                    }

                    Debug.Log("Potion removed from the active inventory slot.");
                    return;
                }
            }
        }

        Debug.LogWarning("No active potion found in the inventory slots!");
    }
}