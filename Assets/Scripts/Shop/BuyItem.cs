using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Für Image

public class BuyItem : MonoBehaviour
{
    [Header("UI-Referenzen")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI errorText;

    // Verwenden wir ein Singleton, also kein Inspector-Reference nötig.
    // Wenn du eine Referenz via Inspector hast, kannst du das auch machen.
    private ActiveInventory activeInventory;

    // ShopItem, das wir kaufen möchten
    [SerializeField] private ShopItem currentShopItem;

    private TextMeshProUGUI coinCounterText;

    private int currentCoins;
    private int price;

    private void Awake()
    {
        // Singleton holen
        activeInventory = ActiveInventory.Instance;
    }

    private void Start()
    {
        // Aktuelles CoinCounter-Textfeld suchen
        coinCounterText = GameObject.Find("Coins Container/Coin Image/CoinCounter (TMP)").GetComponent<TextMeshProUGUI>();
        if (coinCounterText != null)
        {
            currentCoins = int.Parse(coinCounterText.text);
        }
        else
        {
            Debug.LogWarning("CoinCounter (TMP) nicht gefunden!");
        }
    }

    /// <summary>
    ///  Wird von ShopItemUI aufgerufen, damit wir wissen, welches Item wir kaufen sollen.
    /// </summary>
    public void SetCurrentShopItem(ShopItem item)
    {
        currentShopItem = item;

        if (currentShopItem != null)
        {
            nameText.text = currentShopItem.Name;
            priceText.text = currentShopItem.Price.ToString();

            price = currentShopItem.Price;
            Debug.Log($"[BuyItem] SetCurrentShopItem: {currentShopItem.Name}, Preis: {price}");
        }
        else
        {
            Debug.LogWarning("[BuyItem] currentShopItem ist null!");
        }
    }

    /// <summary>
    ///  Button-Click-Methode zum Kauf
    /// </summary>
    public void OnButtonClick()
    {
        if (currentShopItem == null)
        {
            errorText.text = "Kein Item zugewiesen.";
            Debug.LogWarning("[BuyItem] currentShopItem ist null. Abbruch.");
            return;
        }

        Debug.Log($"[BuyItem] Kaufvorgang gestartet für: {currentShopItem.Name} (Preis: {price})");

        if (currentCoins >= price)
        {
            // Versuchen, das Item ins Inventar zu packen
            if (AddItemToInventory(currentShopItem))
            {
                // Kauf erfolgreich
                currentCoins -= price;
                coinCounterText.text = currentCoins.ToString();

                priceText.text = "Gekauft!";

                Debug.Log($"[BuyItem] {currentShopItem.Name} gekauft! Neue Coin-Anzahl: {currentCoins}");
            }
            else
            {
                errorText.text = "Kein Platz im Inventar!";
                Debug.Log($"[BuyItem] Kauf fehlgeschlagen: Kein Platz im Inventar für {currentShopItem.Name}.");
            }
        }
        else
        {
            errorText.text = "Du hast zu wenig Coins!";
            Debug.Log($"[BuyItem] Zu wenig Coins! {currentCoins} < Preis {price}.");
        }
    }

    /// <summary>
    ///  Versucht, das gekaufte Item ins Inventar (ActiveInventory) zu legen.
    ///  Wenn erfolgreich: true, sonst false.
    /// </summary>
    private bool AddItemToInventory(ShopItem shopItem)
    {
        // Debug-Ausgabe
        Debug.Log($"[BuyItem] Prüfe Inventar-Slots für {shopItem.Name}...");

        int slotCount = activeInventory.transform.childCount;

        for (int i = 0; i < slotCount; i++)
        {
            Transform slotTransform = activeInventory.transform.GetChild(i);
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();

            if (slot == null)
            {
                Debug.LogWarning($"[BuyItem] Kind {i} hat keinen InventorySlot! Überspringe...");
                continue;
            }

            // Prüfen, ob der Slot leer ist
            if (slot.GetWeaponInfo() == null)
            {
                // Debug und Setzen
                Debug.Log($"[BuyItem] Freier Slot gefunden bei Index {i}. Lege {shopItem.Name} hinein...");

                // Waffe/Item-Logik (falls dein ShopItem eine WeaponInfo hat)
                slot.SetWeaponInfo(shopItem.weaponInfo);

                if (slotTransform.childCount > 1)
                {
                    Transform itemTransform = slotTransform.GetChild(1); // "Item"
                    Image itemImage = itemTransform.GetComponent<Image>();
                    if (itemImage != null)
                    {
                        itemImage.sprite = shopItem.Icon;
                        Debug.Log($"[BuyItem] Setze Icon von {shopItem.Name} im Inventar-Slot {i}.");
                    }
                    else
                    {
                        Debug.LogWarning($"[BuyItem] Inventar-Slot {i} hat kein Image-Component am 'Item'-Objekt!");
                    }
                }
                else
                {
                    Debug.LogWarning($"[BuyItem] Slot {i} hat nicht die erwartete Hierarchie (kein ChildIndex=1).");
                }

                return true;
            }
        }

        // Kein leerer Slot gefunden
        return false;
    }
}
