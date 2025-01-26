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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "healing")
            {
                playerHealth.HealPlayer(10);
                RemovePotionFromInventory();
                Destroy(gameObject);
            }
        }
    }

    private void RemovePotionFromInventory()
    {
        InventorySlot[] inventorySlots = FindObjectsOfType<InventorySlot>();

        foreach (InventorySlot slot in inventorySlots)
        {
            // Pr?fen, ob der Slot aktiv ist (Child "Active" sichtbar)
            Transform activeChild = slot.transform.Find("Active");
            if (activeChild != null && activeChild.gameObject.activeSelf)
            {
                if (slot.GetWeaponInfo() == weaponInfo)
                {
                    slot.RemoveWeaponInfo();

                    // Bild zur?cksetzen
                    Transform itemChild = slot.transform.Find("Item");
                    if (itemChild != null)
                    {
                        Image itemImage = itemChild.GetComponent<Image>();
                        if (itemImage != null && emptySprite != null)
                        {
                            itemImage.sprite = emptySprite;
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