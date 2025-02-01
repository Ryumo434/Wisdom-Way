using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private TextMeshProUGUI stackCount;

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    // F?ge diese Methode hinzu:
    public void SetWeaponInfo(WeaponInfo newWeaponInfo)
    {
        weaponInfo = newWeaponInfo;
    }

    public void RemoveWeaponInfo()
    {
        weaponInfo = null;
    }

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
}
