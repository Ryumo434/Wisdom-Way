using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button buyButton;

    private ShopItem shopItem;

    private ActiveInventory activeInventory;

    private void Awake()
    {
        activeInventory = ActiveInventory.Instance;
    }

    public void Initialize(ShopItem item)
    {
        BuyItem buyItem = GetComponent<BuyItem>();
        shopItem = item;



        nameText.text = shopItem.Name;

        // checken ob eine waffe im inv ist, wenn ja statt den preis, "gekauft" reinschreiben ----
        InventorySlot[] allSlots = activeInventory.GetComponentsInChildren<InventorySlot>(true);

        bool alreadyOwned = false;
        foreach (var slot in allSlots)
        {
            Debug.Log($"Slot {slot.name} hat Waffe: {slot.GetWeaponInfo()} vs {shopItem.weaponInfo}");
            if (slot.GetWeaponInfo() == shopItem.weaponInfo)
            {
                Debug.Log("GENAU die gleiche Referenz gefunden!");
                alreadyOwned = true;
                break;
            }
        }


        if (alreadyOwned)
        {
            priceText.text = "Gekauft";
        }
        else
        {
            priceText.text = shopItem.Price.ToString();
        }
        // ----

        itemImage.sprite = shopItem.Icon;



        if (buyItem != null)
        {
            buyItem.SetCurrentShopItem(shopItem);
        }
        else
        {
            Debug.LogWarning($"Kein BuyItem-Script am Prefab {gameObject.name} gefunden!");
        }
    }
}