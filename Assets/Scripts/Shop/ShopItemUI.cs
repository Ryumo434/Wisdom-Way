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
        shopItem = item;

        nameText.text = shopItem.Name;
        priceText.text = shopItem.Price.ToString();
        itemImage.sprite = shopItem.Icon;
    }

    private void Awake()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    public void OnBuyButtonClicked()
    {
        BuyItem buyItem = GetComponent<BuyItem>();
        if (buyItem != null)
        {
            buyItem.SetCurrentShopItem(shopItem);
            buyItem.OnButtonClick();
        }
        else
        {
            Debug.LogWarning($"Kein BuyItem-Script am Prefab {gameObject.name} gefunden!");
        }
    }
}