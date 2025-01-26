using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;

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
}
