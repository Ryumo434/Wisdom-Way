using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;

    [SerializeField] private float duration = 5.0f;
    [SerializeField] private float speedMultiplier = 1.5f;

    private PotionEffectManager effectManager;
    private GameObject potionEffectGameObject;

    private void Awake()
    {
        potionEffectGameObject = GameObject.FindWithTag("PotionEffectManager");
        effectManager = potionEffectGameObject.GetComponent<PotionEffectManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponInfo.effect == "speed" && !InventoryModeManager.InventoryIsOpen)
            {
                effectManager.StartSpeedPotionEffect(weaponInfo, duration, speedMultiplier, emptySprite, gameObject);
            }
        }
    }
}