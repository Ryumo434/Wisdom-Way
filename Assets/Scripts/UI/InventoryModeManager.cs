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
        // 1) Escape-Input blocken, wenn Inventar offen ist
        //    (und ggf. Inventar damit schließen, anstatt das Hauptmenü zu öffnen).
        if (isInventoryModeActive && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventoryMode();
            return;
        }

        // 2) Inventar per Taste "I" öffnen/schließen (oder Esc, wenn man das möchte)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryMode();
        }

        // Wenn ein Item "gezogen" wird, soll es der Maus folgen
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
                    // Ganz wichtig: erst "1" setzen, dann unsichtbar machen.
                    selectedSlot.setStackCount("1");
                    selectedSlot.setStackCountInvisible();
                }
            }
            // Wichtig: Nach Abbruch wieder zurücksetzen
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
        // 1) Wenn kein Drag aktiv ist -> Item aus Slot aufnehmen
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

                // Slot vollständig leeren -> auf "0" setzen, da Item rausgenommen
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

                // Falls der aufgenommene Slot der aktive Slot war, erneut prüfen
                if (ActiveInventory.Instance.IsSlotTheActiveOne(clickedSlot))
                {
                    ActiveInventory.Instance.RecheckActiveSlot();
                }
            }
        }
        // 2) Wenn bereits Drag aktiv ist -> Item ablegen oder tauschen
        else
        {
            // Leerer Slot -> einfach ablegen
            if (clickedSlot.GetWeaponInfo() == null)
            {
                clickedSlot.SetWeaponInfo(draggedWeaponInfo);
                clickedSlot.UpdateSlotUI(inventoryEmpty, draggedWeaponInfo.weaponSprite);

                if (draggedStackCount > 1)
                {
                    clickedSlot.setStackCount(draggedStackCount.ToString());
                    clickedSlot.setStackCountVisible();
                }
                else
                {
                    // WICHTIG: erst "1", dann unsichtbar
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

                // Drag-Stack zurücksetzen
                draggedStackCount = 1;

                // Falls in einen aktiven Slot abgelegt
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
                    clickedSlot.setStackCount("1");
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
                    selectedSlot.setStackCount("1");
                    selectedSlot.setStackCountInvisible();
                }

                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                }
                isDragging = false;

                // Drag-Stack zurücksetzen
                draggedStackCount = 1;
                selectedSlot = null;

                // Falls einer der beiden Slots der aktive Slot war
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
        // 1) Wenn wir gerade ein Item ziehen und es löschen wollen
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

        // 2) Falls wir ein Item aus dem Slot löschen wollen
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
                    // Hier entscheidest du, ob das Item bei "1" komplett gelöscht werden soll oder sichtbar bleibt.
                    // Der Code unten sagt, wenn wir auf 1 kommen, Item entfernen (??). 
                    // Vermutlich willst du es beibehalten, also ggf. diesen Code anpassen.

                    // Im Original war "Entgültig löschen", wenn er 1 erreicht – das scheint aber
                    // eher ein Logikfehler zu sein, weil wir es ja noch 1x behalten wollen.
                    // Passe das auf deinen gewünschten Ablauf an:

                    // selectedSlot.RemoveWeaponInfo();
                    // ...

                    // Ich demonstriere hier mal, wie es ansonsten wäre:
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

            // Falls der entfernte Slot der aktive war
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
        // Optional: Hier könntest du den Slot grafisch hervorheben
    }
}