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
        // Falls du hier dynamisch noch etwas einstellen möchtest:
        // (z.B. gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //       gridLayout.constraintCount = 2;)

        PopulateShop();
    }

    // Füllt das Grid mit allen Items aus dem ShopInventory
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
            // Prefab instanziieren
            GameObject newItemUI = Instantiate(shopItemPrefab, contentParent);

            // Optional: Debug-Log
            Debug.Log($"Instantiated {item.Name}");

            // RectTransform prüfen und skalieren
            RectTransform rectTransform = newItemUI.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one; // Sicherstellen, dass die Skalierung korrekt ist
            }

            // Daten setzen
            ShopItemUI itemUI = newItemUI.GetComponent<ShopItemUI>();
            if (itemUI != null)
            {
                itemUI.SetName(item.Name);
                itemUI.SetPrice(item.Price);
                itemUI.SetImage(item.Icon);
            }
        }
    }

}
