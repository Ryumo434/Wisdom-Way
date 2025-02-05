using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}
