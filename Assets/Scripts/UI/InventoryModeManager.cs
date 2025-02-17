using TMPro;
using Unity.VisualScripting;
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryMode();
        }

        // Wenn ein Item genommen wird, dann folgt es dem Mauszeiger
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

                // StackCount wiederherstellen
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
        if (!isDragging)
        {
            if (clickedSlot.GetWeaponInfo() != null)
            {
                // Ursprungsslot und das Item merken
                selectedSlot = clickedSlot;
                draggedWeaponInfo = clickedSlot.GetWeaponInfo();

                // StackCount auslesen
                if (int.TryParse(clickedSlot.getStackCount().text, out int count))
                {
                    draggedStackCount = count;
                }
                else
                {
                    draggedStackCount = 1;
                }

                clickedSlot.setStackCount("1");
                clickedSlot.RemoveWeaponInfo();
                clickedSlot.UpdateSlotUI(inventoryEmpty, null);
                clickedSlot.setStackCountInvisible();

                if (draggedItemImage != null)
                {
                    draggedItemImage.sprite = draggedWeaponInfo.weaponSprite;
                    draggedItemImage.gameObject.SetActive(true);
                }

                isDragging = true;

                // +++ NEU: Wenn der Slot der aktive Slot ist, ActiveInventory updaten
                if (ActiveInventory.Instance.IsSlotTheActiveOne(clickedSlot))
                {
                    ActiveInventory.Instance.RecheckActiveSlot();
                }
            }
        }
        else
        {
            if (clickedSlot.GetWeaponInfo() == null)
            {
                // Leerer Slot: einfach ablegen
                clickedSlot.SetWeaponInfo(draggedWeaponInfo);
                clickedSlot.UpdateSlotUI(inventoryEmpty, draggedWeaponInfo.weaponSprite);
                if (draggedStackCount > 1)
                {
                    clickedSlot.setStackCount(draggedStackCount.ToString());
                    clickedSlot.setStackCountVisible();
                }
                else
                {
                    clickedSlot.setStackCountInvisible();
                }

                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                }
                isDragging = false;
                selectedSlot = null;

                // +++ NEU: Falls man in einen aktiven Slot ablegt
                if (ActiveInventory.Instance.IsSlotTheActiveOne(clickedSlot))
                {
                    ActiveInventory.Instance.RecheckActiveSlot();
                }
            }
            else
            {
                // Ziel-Slot enthält bereits ein Item -> Swap
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
                    clickedSlot.setStackCountInvisible();
                }

                // Das ursprünglich im Ziel-Slot liegende Item wandert in den Ursprungsslot
                selectedSlot.SetWeaponInfo(targetWeapon);
                selectedSlot.UpdateSlotUI(inventoryEmpty, targetWeapon.weaponSprite);
                if (targetStackCount > 1)
                {
                    selectedSlot.setStackCount(targetStackCount.ToString());
                    selectedSlot.setStackCountVisible();
                }
                else
                {
                    selectedSlot.setStackCountInvisible();
                }

                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                }
                isDragging = false;
                selectedSlot = null;

                // +++ NEU: Wenn einer der beiden Slots der aktive Slot war
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
        if (isDragging && draggedWeaponInfo != null)
        {
            if (draggedStackCount > 1)
            {
                draggedStackCount--;
            }
            else
            {
                // Nur 1 Item -> komplett löschen
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

        if (selectedSlot != null)
        {
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
                    // Entgültig löschen
                    selectedSlot.RemoveWeaponInfo();
                    Transform itemTransform = selectedSlot.transform.GetChild(1);
                    Image itemImage = itemTransform.GetComponent<Image>();
                    itemImage.sprite = inventoryEmpty;
                    DeselectSlot();
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

            // +++ NEU: Falls der entfernte Slot der aktive war
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
            HighlightSlot(selectedSlot, false);
            selectedSlot = null;
            if (draggedItemImage != null)
            {
                draggedItemImage.sprite = inventoryEmpty;
            }
        }
    }


    private void HighlightSlot(InventorySlot slot, bool highlight)
    {
        // Hier könntest du z. B. den Slot visuell hervorheben,
        // wenn er ausgewählt ist. Aktuell leer.
    }
}