using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "healing" && !InventoryModeManager.InventoryIsOpen)
            {
                PotionEffectManager.Instance.UseHealPotion(weaponInfo, emptySprite, gameObject);
            }
        }
    }
}