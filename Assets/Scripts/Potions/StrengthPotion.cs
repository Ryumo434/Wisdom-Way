using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StrengthPotion : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Sprite emptySprite;
    public static bool hasStrength;

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
            if (weaponInfo.effect == "strength" && !InventoryModeManager.InventoryIsOpen)
            {
                effectManager.StartStrengthPotionEffect(weaponInfo, 11.0f, emptySprite, gameObject);
            }
        }
    }
}