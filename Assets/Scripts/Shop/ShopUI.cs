using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics;

public class ShopUI : MonoBehaviour
{
    [Header("Grid/ScrollView-Einstellungen")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GridLayoutGroup gridLayout;

    [Header("Prefab-Einstellungen")]
    [SerializeField] private GameObject shopItemPrefab;

    [Header("Inventar-Einstellungen")]
    [SerializeField] private List<ShopItem> shopInventory;

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        // Vorherigen Inhalt entfernen
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Shop-Items hinzufügen
        foreach (ShopItem item in shopInventory)
        {
            GameObject newItemUI = Instantiate(shopItemPrefab, contentParent);

            ShopItemUI itemUI = newItemUI.GetComponent<ShopItemUI>();
            if (itemUI != null)
            {
                // Hier kriegt das UI seine Daten
                itemUI.Initialize(item);
            }
        }
    }
    public void OnButtonClick(Button clickedButton)
    {
        if (clickedButton.tag == "weapon")
        {
            filterWeapons();
        } else if (clickedButton.tag == "potion")
        {
            filterPotions();
        } else
        {
            PopulateShop();
        }
    }

    private void filterPotions()
    {
        // Vorherigen Inhalt entfernen
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Shop-Items hinzufügen
        foreach (ShopItem item in shopInventory)
        {
            if (item.Description == "potion")
            {
                GameObject newItemUI = Instantiate(shopItemPrefab, contentParent);

                // ShopItemUI-Komponente holen
                ShopItemUI itemUI = newItemUI.GetComponent<ShopItemUI>();
                if (itemUI != null)
                {
                    // Hier kriegt das UI seine Daten
                    itemUI.Initialize(item);
                }
            }
        }
    }

    private void filterWeapons()
    {
        // Vorherigen Inhalt entfernen
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Shop-Items hinzufügen
        foreach (ShopItem item in shopInventory)
        {
            if (item.Description == "weapon")
            {
                GameObject newItemUI = Instantiate(shopItemPrefab, contentParent);

                // ShopItemUI-Komponente holen
                ShopItemUI itemUI = newItemUI.GetComponent<ShopItemUI>();
                if (itemUI != null)
                {
                    // Hier kriegt das UI seine Daten
                    itemUI.Initialize(item);
                }
            }
        }
    }
}
