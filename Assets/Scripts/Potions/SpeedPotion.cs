using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;

    [SerializeField] private float duration = 5.0f;
    [SerializeField] private float speedMultiplier = 1.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "speed")
            {
                PotionEffectManager.Instance.StartSpeedPotionEffect(weaponInfo, duration, speedMultiplier, emptySprite);
                Destroy(gameObject);
            }
        }
    }
}