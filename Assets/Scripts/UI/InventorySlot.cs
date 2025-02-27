using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private TextMeshProUGUI stackCount;
    [SerializeField] private TextMeshProUGUI potionTimer;

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void SetWeaponInfo(WeaponInfo newWeaponInfo)
    {
        weaponInfo = newWeaponInfo;
    }

    public void RemoveWeaponInfo()
    {
        weaponInfo = null;
    }

    public string getItemName()
    {
        return weaponInfo != null ? weaponInfo.name : "EMPTY_SLOT";
    }


    // ----- Stack Counter -----

    public TextMeshProUGUI getStackCount()
    {
        return stackCount;
    }

    public void setStackCount(string newValue)
    {
        stackCount.text = newValue;
    }

    public void setStackCountVisible()
    {
        stackCount.gameObject.SetActive(true);
    }

    public void setStackCountInvisible()
    {
        stackCount.gameObject.SetActive(false);
    }

    // ----- Timer -----

    public void setTimer(string newValue)
    {
        potionTimer.text = newValue;
    }

    public void setTimertVisible()
    {
        potionTimer.gameObject.SetActive(true);
    }

    public void setTimerInvisible()
    {
        potionTimer.gameObject.SetActive(false);
    }

    // ----- Aktualisieren des UI -----
    public void UpdateSlotUI(Sprite emptySprite, Sprite selectedSprite)
    {
        Image slotIcon = transform.GetChild(1).GetComponent<Image>();

        if (weaponInfo == null)
        {
            // Kein Item -> z.B. leerer Sprite
            if (slotIcon != null && emptySprite != null)
            {
                slotIcon.sprite = emptySprite;
            }
            else if (slotIcon != null)
            {
                slotIcon.sprite = null;
            }

            // Stack- und Timer-Anzeigen ausblenden
            setStackCountInvisible();
            setTimerInvisible();
        }
        else
        {
            // Item vorhanden -> Sprite setzen
            if (slotIcon != null && selectedSprite != null)
            {
                slotIcon.sprite = selectedSprite;
            }

            // Hier könntest du z.B. Stackgröße und Timer anzeigen,
            // wenn deine WeaponInfo diese Informationen enthält.
        }
    }
}
