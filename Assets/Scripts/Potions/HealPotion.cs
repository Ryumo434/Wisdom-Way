using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private int healAmount = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "healing")
            {
                PotionEffectManager.Instance.UseHealPotion(weaponInfo, healAmount, emptySprite);
                // Zerstöre das Potion-Objekt
                Destroy(gameObject);
            }
        }
    }
}