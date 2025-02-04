using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionEffectManager : MonoBehaviour
{
    public static PotionEffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartStrengthPotionEffect(WeaponInfo weaponInfo, float duration, Sprite emptySprite)
    {
        StartCoroutine(StrengthPotionEffectCoroutine(weaponInfo, duration, emptySprite));
    }

    private IEnumerator StrengthPotionEffectCoroutine(WeaponInfo weaponInfo, float duration, Sprite emptySprite)
    {
        StrengthPotion.hasStrength = true;
        Debug.Log("Strength effect started: " + StrengthPotion.hasStrength);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            Debug.Log("Elapsed (strength): " + elapsed);
            elapsed += Time.deltaTime;
        }

        StrengthPotion.hasStrength = false;
        Debug.Log("Strength effect ended: " + StrengthPotion.hasStrength);
        RemovePotionFromInventory(weaponInfo, emptySprite);
    }

    public void StartSpeedPotionEffect(WeaponInfo weaponInfo, float duration, float speedMultiplier, Sprite emptySprite)
    {
        StartCoroutine(SpeedPotionEffectCoroutine(weaponInfo, duration, speedMultiplier, emptySprite));
    }

    private IEnumerator SpeedPotionEffectCoroutine(WeaponInfo weaponInfo, float duration, float speedMultiplier, Sprite emptySprite)
    {
        // Erhöhe die Geschwindigkeit
        PlayerController.Instance.setMoveSpeed(speedMultiplier);
        Debug.Log("SpeedPotion aktiviert: Geschwindigkeit erhöht!");

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }

        // Setze die Geschwindigkeit wieder auf Standard
        PlayerController.Instance.setMoveSpeed(1.0f);
        Debug.Log("SpeedPotion Effekt abgelaufen: Geschwindigkeit zurückgesetzt.");

        RemovePotionFromInventory(weaponInfo, emptySprite);
    }

    public void UseHealPotion(WeaponInfo weaponInfo, int healAmount, Sprite emptySprite)
    {
        // Heile den Spieler
        PlayerHealth.Instance.HealPlayer(healAmount);
        Debug.Log("HealPotion aktiviert: Spieler geheilt um " + healAmount + " HP.");
        RemovePotionFromInventory(weaponInfo, emptySprite);
    }

    private void RemovePotionFromInventory(WeaponInfo weaponInfo, Sprite emptySprite)
    {
        // Suche alle InventorySlots (auch inaktive)
        InventorySlot[] inventorySlots = FindObjectsOfType<InventorySlot>(true);

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetWeaponInfo() == weaponInfo)
            {
                // Aktualisiere den StackCount
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

                // Ändere das angezeigte Bild im Slot
                Transform itemChild = slot.transform.Find("Item");
                if (itemChild != null)
                {
                    Image itemImage = itemChild.GetComponent<Image>();
                    if (itemImage != null && emptySprite != null)
                    {
                        slot.RemoveWeaponInfo();
                        itemImage.sprite = emptySprite;
                    }
                }

                Debug.Log("Potion removed from the active inventory slot.");
                return;
            }
        }

        Debug.LogWarning("No active potion found in the inventory slots!");
    }
}