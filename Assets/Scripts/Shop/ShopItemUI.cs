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

    public void Initialize(ShopItem item)
    {
        BuyItem buyItem = GetComponent<BuyItem>();
        shopItem = item;

        nameText.text = shopItem.Name;
        priceText.text = shopItem.Price.ToString();
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