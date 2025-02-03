using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;

    private PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "speed")
            {
                StartCoroutine(SpeedPotionEffect(5.0f, 1.5f));
            }
        }
    }

    public IEnumerator SpeedPotionEffect(float duration, float speedMultiplier)
    {
        // Erhöhe die Geschwindigkeit
        playerController.setMoveSpeed(speedMultiplier);
        Debug.Log("SpeedPotion aktiviert: Geschwindigkeit erhöht!");

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }


        // Setze die Geschwindigkeit wieder auf Standard
        playerController.setMoveSpeed(1.0f);
        Debug.Log("SpeedPotion Effekt abgelaufen: Geschwindigkeit zurückgesetzt.");

        RemovePotionFromInventory();
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
                            slot.RemoveWeaponInfo();
                            itemImage.sprite = emptySprite;
                            // Zerstöre das Potion-Objekt erst, nachdem der Effekt vollständig abgelaufen ist.
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