using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button buyButton;

    // Referenz auf das Item, das dieses UI-Element darstellt
    private ShopItem shopItem;

    // Wird von ShopUI aufgerufen, um die ShopItem-Daten zu setzen
    public void Initialize(ShopItem item)
    {
        shopItem = item;

        // UI setzen
        nameText.text = shopItem.Name;
        priceText.text = shopItem.Price.ToString();
        itemImage.sprite = shopItem.Icon;
    }

    // Beispiel innerhalb ShopItemUI:
    private void Awake()
    {
        // Referenz zu Button wird ?ber [SerializeField] oder GetComponent<Button>() geholt
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    // Wird aufgerufen, wenn der "Kaufen"-Button gedr?ckt wird
    public void OnBuyButtonClicked()
    {
        // Wir holen das BuyItem-Script (das muss auf demselben Prefab liegen oder einem Child)
        BuyItem buyItem = GetComponent<BuyItem>();
        if (buyItem != null)
        {
            // Jetzt ?bergeben wir das aktuelle ShopItem an BuyItem
            buyItem.SetCurrentShopItem(shopItem);
            // Und starten den Kauf
            buyItem.OnButtonClick();
        }
        else
        {
            Debug.LogWarning($"Kein BuyItem-Script am Prefab {gameObject.name} gefunden!");
        }
    }
}