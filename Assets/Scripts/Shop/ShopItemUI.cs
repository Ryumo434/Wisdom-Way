using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI nameText;

    // Referenz auf das ShopItem (wird beim Erstellen zugewiesen)
    private ShopItem shopItem;

    public void Initialize(ShopItem item)
    {
        shopItem = item;
        SetName(item.Name); // da
        SetPrice(item.Price);
        SetImage(item.Icon);
    }

    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    public void SetPrice(int newPrice)
    {
        priceText.text = newPrice.ToString();
    }

    public void SetImage(Sprite newSprite)
    {
        itemImage.sprite = newSprite;
    }

    // Nehmen wir an, der Button hängt am gleichen GameObject:
    public void OnBuyButtonClicked()
    {
        // Hole dir das BuyItem-Skript (falls auf dem gleichen GameObject hängt)
        BuyItem buyItem = GetComponent<BuyItem>();
        if (buyItem != null)
        {
            // Setze das ShopItem
            buyItem.SetCurrentShopItem(shopItem);
            // Führe gleich den Kaufversuch aus
            buyItem.OnButtonClick();
        }
    }
}
