using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;

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
            if (weaponInfo.effect == "healing" && !InventoryModeManager.InventoryIsOpen)
            {
                effectManager.UseHealPotion(weaponInfo, emptySprite, gameObject);
            }
        }
    }
}