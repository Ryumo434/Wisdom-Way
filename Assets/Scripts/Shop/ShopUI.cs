using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [Header("Grid/ScrollView-Einstellungen")]
    [SerializeField] private Transform contentParent;  // Z.B. das "Content"-Objekt der ScrollView
    [SerializeField] private GridLayoutGroup gridLayout; // Referenz auf das GridLayoutGroup-Objekt

    [Header("Prefab-Einstellungen")]
    [SerializeField] private GameObject shopItemPrefab; // Unser ShopItemUI-Prefab

    [Header("Inventar-Einstellungen")]
    [SerializeField] private List<ShopItem> shopInventory; // Deine Shop-Items

    private void Start()
    {
        PopulateShop();
    }

    // F?llt das Grid mit allen Items aus dem ShopInventory
    private void PopulateShop()
    {
        // Vorherigen Inhalt entfernen
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Shop-Items hinzuf?gen
        foreach (ShopItem item in shopInventory)
        {
            // Prefab instanziieren
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
