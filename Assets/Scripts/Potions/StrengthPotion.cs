using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StrengthPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;
    public static bool hasStrength;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "strength")
            {
                PotionEffectManager.Instance.StartStrengthPotionEffect(weaponInfo, 5.0f, emptySprite, gameObject);
            }
        }
    }
}