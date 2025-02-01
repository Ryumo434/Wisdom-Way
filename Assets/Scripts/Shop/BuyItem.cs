using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    [Header("UI-Referenzen")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI errorText;

    private ActiveInventory activeInventory;

    [SerializeField] private ShopItem currentShopItem;

    private int price;

    private void Awake()
    {
        activeInventory = ActiveInventory.Instance;
    }

    private void Start()
    {
        if (ScoreManager.instance == null)
        {
            Debug.LogError("ScoreManager ist nicht verfügbar!");
        }
    }

    public void SetCurrentShopItem(ShopItem item)
    {
        currentShopItem = item;

        if (currentShopItem != null)
        {
            nameText.text = currentShopItem.Name;
            priceText.text = currentShopItem.Price.ToString();
            price = currentShopItem.Price;
        }
        else
        {
            Debug.LogWarning("[BuyItem] currentShopItem ist null!");
        }
    }

    public void OnButtonClick()
    {
        if (currentShopItem == null)
        {
            errorText.text = "Kein Item zugewiesen.";
            return;
        }

        int currentCoins = ScoreManager.instance.GetScore(); // Aktuellen Punktestand holen

        if (currentCoins >= price)
        {
            if (AddItemToInventory(currentShopItem))
            {
                currentCoins -= price; // Punktestand reduzieren
                ScoreManager.instance.SetScore(currentCoins); // Punktestand aktualisieren
                Debug.Log($"[BuyItem] {currentShopItem.Name} gekauft! Neue Coin-Anzahl: {currentCoins}");
            }
            else
            {
                errorText.text = "Kein Platz im Inventar!";
            }
        }
        else
        {
            errorText.text = "Du hast zu wenig Coins!";
        }
    }


    private bool AddItemToInventory(ShopItem shopItem)
    {
        int slotCount = activeInventory.transform.childCount;

        for (int i = 0; i < slotCount; i++)
        {
            Transform slotTransform = activeInventory.transform.GetChild(i);
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();

            if (slot == null)
                continue;

            //checkt ob das Item schon im Inventar ist
            if (slot.GetWeaponInfo() == shopItem.weaponInfo)
            {
                TextMeshProUGUI currentStackCount = slot.getStackCount();
                int newStackCount = int.Parse(currentStackCount.text) + 1;
                slot.setStackCount(newStackCount.ToString());
                slot.setStackCountVisible();
            }

            // Füge nur hinzu, wenn der Slot leer ist
            if (slot.GetWeaponInfo() == null)
            {
                slot.SetWeaponInfo(shopItem.weaponInfo);

                if (slotTransform.childCount > 1)
                {
                    Transform itemTransform = slotTransform.GetChild(1);
                    Image itemImage = itemTransform.GetComponent<Image>();
                    if (itemImage != null)
                    {
                        itemImage.sprite = shopItem.Icon;
                    }
                }

                Debug.Log($"Potion added to inventory slot {i}.");
                return true;
            }
        }

        Debug.LogWarning("No empty inventory slot found!");
        return false; // Kein freier Slot gefunden
    }
}