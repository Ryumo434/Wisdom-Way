using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryModeManager : MonoBehaviour
{
    /// <summary>
    /// Globales Flag, das anzeigt, ob das Inventar aktuell offen ist.
    /// Andere Skripte (z.B. Waffennutzung, Tranknutzung) können dieses abfragen,
    /// um Aktionen zu blockieren.
    /// </summary>
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
    private int draggedStackCount = 1;  // Wird beim Aufnehmen eines Items gesetzt

    private void Awake()
    {
        inventoryUIPanel.SetActive(false);
        if (draggedItemImage != null)
        {
            draggedItemImage.gameObject.SetActive(false);
        }
        InventoryIsOpen = false; // Zu Beginn ist das Inventar geschlossen
    }

    void Update()
    {
        // Inventar per Taste "I" toggeln
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryMode();
        }

        // Wenn wir ein Item ziehen, soll das Item-Bild dem Mauszeiger folgen
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

        // Zeit anhalten, solange Inventar offen ist
        Time.timeScale = isInventoryModeActive ? 0 : 1;

        if (isInventoryModeActive)
        {
            // Inventar geöffnet
            InventoryIsOpen = true;
        }
        else
        {
            // Inventar geschlossen
            InventoryIsOpen = false;
            CancelDragging();
            DeselectSlot();
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn das Inventar geschlossen wird oder der Spieler
    /// den Drag-Vorgang abbricht.
    /// </summary>
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

    /// <summary>
    /// Wird aufgerufen, wenn ein Inventar-Slot geklickt wird.
    /// </summary>
    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        // Falls gerade kein Drag aktiv ist, starten wir hier den Drag
        if (!isDragging)
        {
            if (clickedSlot.GetWeaponInfo() != null)
            {
                // Merke dir den Ursprungsslot und das Item
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

                // Setze im aktuellen Slot den sichtbaren StackCount auf 1
                clickedSlot.setStackCount("1");

                // Entferne das Item aus dem Slot (UI-Update auf leer)
                clickedSlot.RemoveWeaponInfo();
                clickedSlot.UpdateSlotUI(inventoryEmpty, null);
                clickedSlot.setStackCountInvisible();

                // Zeige das Sprite unter dem Mauszeiger an
                if (draggedItemImage != null)
                {
                    draggedItemImage.sprite = draggedWeaponInfo.weaponSprite;
                    draggedItemImage.gameObject.SetActive(true);
                }

                isDragging = true;
            }
        }
        else
        {
            // Ein Drag ist aktiv -> versuche, das Item abzulegen
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
            }
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn der User auf den "Mülleimer" klickt.
    /// Hier wird das Item bzw. ein Item aus dem Stack gelöscht.
    /// 
    /// Verhalten:
    /// - Ist ein Item im Drag und hat StackCount > 1, wird nur 1 abgezogen
    /// - Ist ein Item im Drag mit StackCount == 1, wird es entfernt
    /// - Liegt das Item im Slot und hat StackCount > 1, wird nur 1 abgezogen
    /// - Liegt das Item im Slot mit StackCount == 1, wird es entfernt
    /// </summary>
    public void OnTrashCanClicked()
    {
        // 1) Falls gerade ein Item gezogen (Drag) wird
        if (isDragging && draggedWeaponInfo != null)
        {
            if (draggedStackCount > 1)
            {
                // StackCounter verringern
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
            return; // Fertig
        }

        // 2) Kein Drag aktiv, aber ein Slot ist ausgewählt
        if (selectedSlot != null)
        {
            int currentCount = 1;
            if (int.TryParse(selectedSlot.getStackCount().text, out int parsedCount))
            {
                currentCount = parsedCount;
            }

            if (currentCount > 1)
            {
                // Stack verringern
                currentCount--;
                selectedSlot.setStackCount(currentCount.ToString());

                // Nur wenn weiter >1 übrig ist, soll es sichtbar bleiben.
                // Falls genau 1 übrig bleibt, behalten wir es. (Siehe nächster Satz?)
                // Achtung: Du kannst hier entscheiden, ob 1 Item noch bleiben soll oder nicht.
                // Der User sagte: "wenn es nur 1 Item gibt, soll es gelöscht werden."
                // => Also wenn nach dem Abziehen 1 übrig bleibt, wird es ebenfalls gelöscht.
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
                    // StackCount > 1 -> sichtbar lassen
                    selectedSlot.setStackCountVisible();
                }
            }
            else
            {
                // currentCount == 1 => komplett löschen
                selectedSlot.RemoveWeaponInfo();
                Transform itemTransform = selectedSlot.transform.GetChild(1);
                Image itemImage = itemTransform.GetComponent<Image>();
                itemImage.sprite = inventoryEmpty;
                DeselectSlot();
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
