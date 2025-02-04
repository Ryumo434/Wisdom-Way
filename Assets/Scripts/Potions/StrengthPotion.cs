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
                // Starte den Potion-Effekt über den Manager
                PotionEffectManager.Instance.StartStrengthPotionEffect(weaponInfo, 5.0f, emptySprite);

                // Optional: Zerstöre das Potion-Objekt, damit es nicht doppelt verarbeitet wird
                Destroy(gameObject);
            }
        }
    }
}