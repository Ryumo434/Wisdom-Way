using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyItem : MonoBehaviour
{
    [Header("UI-Referenzen")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI errorText;

    private ActiveInventory activeInventory;

    [SerializeField] private ShopItem currentShopItem;

    private int price;

    private void Awake()
    {
        activeInventory = ActiveInventory.Instance;
    }

    private void Start()
    {
        if (ScoreManager.instance == null)
        {
            Debug.LogError("ScoreManager ist nicht verfügbar!");
        }
    }

    public void SetCurrentShopItem(ShopItem item)
    {
        currentShopItem = item;

        if (currentShopItem != null)
        {
            nameText.text = currentShopItem.Name;
            priceText.text = currentShopItem.Price.ToString();
            price = currentShopItem.Price;
        }
        else
        {
            Debug.LogWarning("[BuyItem] currentShopItem ist null!");
        }
    }

    public void OnButtonClick()
    {
        Debug.Log($"[OnButtonClick] wurde aufgerufen von: {gameObject.name}", gameObject);

        if (currentShopItem == null)
        {
            StartCoroutine(ErrorMessageRoutine("Kein Item zugewiesen!"));
            return;
        }

        int currentCoins = ScoreManager.instance.GetScore();

        if (currentCoins >= price)
        {
            if (AddItemToInventory(currentShopItem))
            {
                currentCoins -= price;
                ScoreManager.instance.SetScore(currentCoins);
                Debug.Log($"[BuyItem] {currentShopItem.Name} gekauft! Neue Coin-Anzahl: {currentCoins}", gameObject);
            }
            else
            {
                StartCoroutine(ErrorMessageRoutine("Kein Platz im Inventar!"));
            }
        }
        else
        {
            StartCoroutine(ErrorMessageRoutine("Du hast zu wenig Coins!"));
        }
    }



    private bool AddItemToInventory(ShopItem shopItem)
    {
        int slotCount = activeInventory.transform.childCount;

        for (int i = 0; i < slotCount; i++)
        {
            Transform slotTransform = activeInventory.transform.GetChild(i);
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();

            if (slot == null)
                continue;

            //checkt ob das Item schon im Inventar ist
            if (slot.GetWeaponInfo() == shopItem.weaponInfo)
            {
                if (slot.GetWeaponInfo().isUnique)
                {
                    return false;
                }
                TextMeshProUGUI currentStackCount = slot.getStackCount();
                int newStackCount = int.Parse(currentStackCount.text) + 1;
                slot.setStackCount(newStackCount.ToString());
                slot.setStackCountVisible();
                return true;
            }

            // Füge nur hinzu, wenn der Slot leer ist
            if (slot.GetWeaponInfo() == null)
            {
                slot.SetWeaponInfo(shopItem.weaponInfo);

                if (slotTransform.childCount > 1)
                {
                    Transform itemTransform = slotTransform.GetChild(1);
                    Image itemImage = itemTransform.GetComponent<Image>();
                    if (itemImage != null)
                    {
                        itemImage.sprite = shopItem.Icon;
                    }

                    // überprüfen ob Item unique ist und button auf "gekauft" stellen
                    if (slot.GetWeaponInfo().isUnique)
                    {
                        priceText.text = "gekauft";
                    }
                }

                Debug.Log($"Potion added to inventory slot {i}.");
                return true;
            }
        }

        Debug.LogWarning("No empty inventory slot found!");
        return false;
    }

    private IEnumerator ErrorMessageRoutine(string text)
    {
        Debug.Log("Show error Message 5 sec...");
        errorText.text = text;
        errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        errorText.gameObject.SetActive(false);
        Debug.Log("Deactivating...");
    }

    void OnDisable()
    {
        errorText.gameObject.SetActive(false);
    }
}