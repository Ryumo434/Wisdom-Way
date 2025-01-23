using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI nameText;

    // Setzt den Name
    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    // Setzt den Preis
    public void SetPrice(float newPrice)
    {
        priceText.text = newPrice.ToString();
    }

    // Setzt das Bild
    public void SetImage(Sprite newSprite)
    {
        itemImage.sprite = newSprite;
    }
}
