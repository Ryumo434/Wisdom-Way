using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private List<ShopItem> allShopItems; // Liste aller Shop-Items als ScriptableObjects

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

   public ShopItem FindShopItemByName(string itemName)
    {
        ShopItem foundItem = allShopItems.FirstOrDefault(item => item.Name == itemName);
        if (foundItem == null)
        {
            Debug.LogError($"Item '{itemName}' wurde nicht im Shop gefunden!");
        }
        return foundItem;
    }
}

