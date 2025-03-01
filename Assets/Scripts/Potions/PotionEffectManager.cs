using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PotionEffectManager : MonoBehaviour
{
    public static PotionEffectManager Instance { get; private set; }


    public void StartStrengthPotionEffect(WeaponInfo weaponInfo, float duration, Sprite emptySprite, GameObject go)
    {
        StartCoroutine(StrengthPotionEffectCoroutine(weaponInfo, duration, emptySprite, go));
    }

    private IEnumerator StrengthPotionEffectCoroutine(WeaponInfo weaponInfo, float duration, Sprite emptySprite, GameObject go)
    {
        StrengthPotion.hasStrength = true;
        Debug.Log("Strength effect started: " + StrengthPotion.hasStrength);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            setPotionTimer(weaponInfo, duration - elapsed);
            elapsed += Time.deltaTime;
        }

        StrengthPotion.hasStrength = false;
        Debug.Log("Strength effect ended: " + StrengthPotion.hasStrength);
        HidePotionTimer(weaponInfo);
        RemovePotionFromInventory(weaponInfo, emptySprite, go);
    }

    public void StartSpeedPotionEffect(WeaponInfo weaponInfo, float duration, float speedMultiplier, Sprite emptySprite, GameObject go)
    {
        StartCoroutine(SpeedPotionEffectCoroutine(weaponInfo, duration, speedMultiplier, emptySprite, go));
    }

    private IEnumerator SpeedPotionEffectCoroutine(WeaponInfo weaponInfo, float duration, float speedMultiplier, Sprite emptySprite, GameObject go)
    {
        PlayerController.Instance.setMoveSpeed(speedMultiplier);
        Debug.Log("SpeedPotion aktiviert: Geschwindigkeit erhöht!");

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            setPotionTimer(weaponInfo, duration - elapsed);
            elapsed += Time.deltaTime;
        }

        PlayerController.Instance.setMoveSpeed(1.0f);
        Debug.Log("SpeedPotion Effekt abgelaufen: Geschwindigkeit zurückgesetzt.");
        HidePotionTimer(weaponInfo);
        RemovePotionFromInventory(weaponInfo, emptySprite, go);
    }

    public void UseHealPotion(WeaponInfo weaponInfo, Sprite emptySprite, GameObject go)
    {
        int maxHealth = 20;
        int currentHealth = PlayerHealth.Instance.getCurrentPlayerHealth();
        int missingHealth = maxHealth - currentHealth;
        int amountToHeal = 10;

        if (missingHealth < 10)
        {
            amountToHeal = missingHealth;
        }

        if (amountToHeal <= 0)
        {
            Debug.Log("HealPotion: Spieler ist fast voll, keine Heilung nötig.");
            RemovePotionFromInventory(weaponInfo, emptySprite, go);
            return;
        }

        PlayerHealth.Instance.HealPlayer(amountToHeal);
        Debug.Log("HealPotion aktiviert: Spieler geheilt um " + amountToHeal + " HP.");

        RemovePotionFromInventory(weaponInfo, emptySprite, go);
    }

    private void setPotionTimer(WeaponInfo weaponInfo, float elapsed)
    {
        InventorySlot[] inventorySlots = FindObjectsOfType<InventorySlot>(true);

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetWeaponInfo() == weaponInfo)
            {
                slot.setTimertVisible();
                slot.setTimer(elapsed.ToString("F1"));
            }
        }

        
    }

    private void HidePotionTimer(WeaponInfo weaponInfo)
    {
        InventorySlot[] inventorySlots = FindObjectsOfType<InventorySlot>(true);

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetWeaponInfo() == weaponInfo)
            {
                slot.setTimerInvisible();
            }
        }
    }


    private void RemovePotionFromInventory(WeaponInfo weaponInfo, Sprite emptySprite, GameObject go)
    {
        InventorySlot[] inventorySlots = FindObjectsOfType<InventorySlot>(true);

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetWeaponInfo() == weaponInfo)
            {
                if (slot.getStackCount().text != "1")
                {
                    int newValue = int.Parse(slot.getStackCount().text) - 1;
                    slot.setStackCount(newValue.ToString());

                    if (newValue == 0)
                    {
                        slot.setStackCountInvisible();
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    slot.setStackCountInvisible();
                }

                Transform itemChild = slot.transform.Find("Item");
                if (itemChild != null)
                {
                    Image itemImage = itemChild.GetComponent<Image>();
                    if (itemImage != null && emptySprite != null)
                    {
                        slot.setTimerInvisible();
                        slot.RemoveWeaponInfo();
                        itemImage.sprite = emptySprite;
                        Destroy(go);
                    }
                }

                Debug.Log("Potion removed from the active inventory slot.");
                return;
            }
        }

        Debug.LogWarning("No active potion found in the inventory slots!");
    }
}