using UnityEngine;
using UnityEngine.UI;

public class InventoryModeManager : MonoBehaviour
{
    public static bool InventoryIsOpen = false;

    [Header("UI Referenzen")]
    public GameObject inventoryUIPanel;
    public Sprite inventoryEmpty;
    public Image draggedItemImage;
    public Canvas mainCanvas;

    private bool isInventoryModeActive = false;
    private InventorySlot selectedSlot = null;
    private WeaponInfo draggedWeaponInfo = null;
    private bool isDragging = false;
    private int draggedStackCount = 1;

    private void Awake()
    {
        inventoryUIPanel.SetActive(false);
        if (draggedItemImage != null)
        {
            draggedItemImage.gameObject.SetActive(false);
        }
        InventoryIsOpen = false;
    }

    void Update()
    {
        // Escape schließt nur Inventar, wenn Inventar offen ist
        if (isInventoryModeActive && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventoryMode();
            return;
        }

        // Taste I zum Öffnen/Schließen
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryMode();
        }

        // Falls wir gerade ein Item ziehen: Image folgt der Maus
        if (isDragging && draggedItemImage != null)
        {
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mainCanvas.transform as RectTransform,
                mousePos,
                mainCanvas.worldCamera,
                out Vector2 localPoint
            );
            draggedItemImage.rectTransform.localPosition = localPoint;
        }
    }

    private void ToggleInventoryMode()
    {
        isInventoryModeActive = !isInventoryModeActive;
        inventoryUIPanel.SetActive(isInventoryModeActive);

        Time.timeScale = isInventoryModeActive ? 0 : 1;

        if (isInventoryModeActive)
        {
            InventoryIsOpen = true;
        }
        else
        {
            InventoryIsOpen = false;
            CancelDragging();
            DeselectSlot();
        }
    }

    private void CancelDragging()
    {
        if (isDragging)
        {
            // Item zurück in den Ursprungsslot
            if (draggedWeaponInfo != null && selectedSlot != null)
            {
                selectedSlot.SetWeaponInfo(draggedWeaponInfo);
                selectedSlot.UpdateSlotUI(inventoryEmpty, draggedWeaponInfo.weaponSprite);

                if (draggedStackCount > 1)
                {
                    selectedSlot.setStackCount(draggedStackCount.ToString());
                    selectedSlot.setStackCountVisible();
                }
                else
                {
                    selectedSlot.setStackCount("1");
                    selectedSlot.setStackCountInvisible();
                }
            }
            draggedStackCount = 1;

            isDragging = false;
            draggedWeaponInfo = null;
            if (draggedItemImage != null)
            {
                draggedItemImage.gameObject.SetActive(false);
                draggedItemImage.sprite = inventoryEmpty;
            }
        }
    }

    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        // 1) Kein Drag -> Item aufnehmen
        if (!isDragging)
        {
            if (clickedSlot.GetWeaponInfo() != null)
            {
                selectedSlot = clickedSlot;
                draggedWeaponInfo = clickedSlot.GetWeaponInfo();

                if (int.TryParse(clickedSlot.getStackCount().text, out int count))
                {
                    draggedStackCount = count;
                }
                else
                {
                    draggedStackCount = 1;
                }

                // Slot leeren, weil Item jetzt "gezogen" wird
                clickedSlot.setStackCount("0");
                clickedSlot.RemoveWeaponInfo();
                clickedSlot.UpdateSlotUI(inventoryEmpty, null);
                clickedSlot.setStackCountInvisible();

                if (draggedItemImage != null)
                {
                    draggedItemImage.sprite = draggedWeaponInfo.weaponSprite;
                    draggedItemImage.gameObject.SetActive(true);
                }

                isDragging = true;

                // Falls der aufgenommene Slot der aktive Slot war -> neu prüfen
                if (ActiveInventory.Instance.IsSlotTheActiveOne(clickedSlot))
                {
                    ActiveInventory.Instance.RecheckActiveSlot();
                }
            }
        }
        // 2) Drag aktiv -> Item ablegen / tauschen
        else
        {
            if (clickedSlot.GetWeaponInfo() == null)
            {
                // Leerer Slot -> Item dort ablegen
                clickedSlot.SetWeaponInfo(draggedWeaponInfo);
                clickedSlot.UpdateSlotUI(inventoryEmpty, draggedWeaponInfo.weaponSprite);

                if (draggedStackCount > 1)
                {
                    clickedSlot.setStackCount(draggedStackCount.ToString());
                    clickedSlot.setStackCountVisible();
                }
                else
                {
                    clickedSlot.setStackCount("1");
                    clickedSlot.setStackCountInvisible();
                }

                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                }
                isDragging = false;
                selectedSlot = null;

                draggedStackCount = 1;

                if (ActiveInventory.Instance.IsSlotTheActiveOne(clickedSlot))
                {
                    ActiveInventory.Instance.RecheckActiveSlot();
                }
            }
            else
            {
                // Ziel-Slot enthält bereits ein Item -> Tauschen (Swap)
                WeaponInfo targetWeapon = clickedSlot.GetWeaponInfo();
                int targetStackCount = 1;
                if (int.TryParse(clickedSlot.getStackCount().text, out int countTarget))
                {
                    targetStackCount = countTarget;
                }

                // Platziere das gezogene Item im Ziel-Slot
                clickedSlot.SetWeaponInfo(draggedWeaponInfo);
                clickedSlot.UpdateSlotUI(inventoryEmpty, draggedWeaponInfo.weaponSprite);

                if (draggedStackCount > 1)
                {
                    clickedSlot.setStackCount(draggedStackCount.ToString());
                    clickedSlot.setStackCountVisible();
                }
                else
                {
                    clickedSlot.setStackCount("1");
                    clickedSlot.setStackCountInvisible();
                }

                // Das alte Item aus dem Ziel-Slot kommt in den Ursprungsslot
                selectedSlot.SetWeaponInfo(targetWeapon);
                selectedSlot.UpdateSlotUI(inventoryEmpty, targetWeapon.weaponSprite);

                if (targetStackCount > 1)
                {
                    selectedSlot.setStackCount(targetStackCount.ToString());
                    selectedSlot.setStackCountVisible();
                }
                else
                {
                    selectedSlot.setStackCount("1");
                    selectedSlot.setStackCountInvisible();
                }

                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                }
                isDragging = false;

                draggedStackCount = 1;
                selectedSlot = null;

                // Falls einer der Slots der aktive Slot war -> neu prüfen
                if (ActiveInventory.Instance.IsSlotTheActiveOne(clickedSlot) ||
                    ActiveInventory.Instance.IsSlotTheActiveOne(selectedSlot))
                {
                    ActiveInventory.Instance.RecheckActiveSlot();
                }
            }
        }
    }

    public void OnTrashCanClicked()
    {
        // ===============================
        // Fall 1: Wir ziehen gerade ein Item
        // ===============================
        if (isDragging && draggedWeaponInfo != null)
        {
            // => Wenn Item "unique" ist, brich ab
            if (draggedWeaponInfo.isUnique)
            {
                Debug.Log("Dieses Item ist einzigartig und kann nicht gelöscht werden.");
                return;
            }

            // Falls nicht unique -> normaler Ablauf
            if (draggedStackCount > 1)
            {
                draggedStackCount--;
            }
            else
            {
                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                    draggedItemImage.sprite = inventoryEmpty;
                }
                isDragging = false;
            }
            return;
        }

        // ===============================
        // Fall 2: Wir haben einen Slot ausgewählt
        // ===============================
        if (selectedSlot != null)
        {
            // Prüfe, ob das Item unique ist:
            WeaponInfo slotWeapon = selectedSlot.GetWeaponInfo();
            if (slotWeapon != null && slotWeapon.isUnique)
            {
                Debug.Log("Dieses Item ist einzigartig und kann nicht gelöscht werden.");
                return;
            }

            // Ansonsten normaler Lösch-Ablauf
            int currentCount = 1;
            if (int.TryParse(selectedSlot.getStackCount().text, out int parsedCount))
            {
                currentCount = parsedCount;
            }

            if (currentCount > 1)
            {
                currentCount--;
                selectedSlot.setStackCount(currentCount.ToString());

                if (currentCount == 1)
                {
                    selectedSlot.setStackCount("1");
                    selectedSlot.setStackCountInvisible();
                }
                else
                {
                    selectedSlot.setStackCountVisible();
                }
            }
            else
            {
                selectedSlot.RemoveWeaponInfo();
                Transform itemTransform = selectedSlot.transform.GetChild(1);
                Image itemImage = itemTransform.GetComponent<Image>();
                itemImage.sprite = inventoryEmpty;
                DeselectSlot();
            }

            if (ActiveInventory.Instance.IsSlotTheActiveOne(selectedSlot))
            {
                ActiveInventory.Instance.RecheckActiveSlot();
            }
        }
    }

    private void DeselectSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot = null;
            if (draggedItemImage != null)
            {
                draggedItemImage.sprite = inventoryEmpty;
            }
        }
    }
}