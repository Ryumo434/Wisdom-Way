using UnityEngine;
using UnityEngine.UI;

public class InventoryModeManager : MonoBehaviour
{
    [Header("UI Referenzen")]
    public GameObject inventoryUIPanel;
    public Sprite inventoryEmpty;
    private Image draggedItemImage;
    private GameObject draggedItemGameObject;
    public Canvas mainCanvas;

    private bool isInventoryModeActive = false;
    private InventorySlot selectedSlot = null;
    private WeaponInfo draggedWeaponInfo = null;
    private bool isDragging = false;

    private void Awake()
    {
        draggedItemGameObject = GameObject.Find("Active");
        draggedItemImage = draggedItemGameObject.GetComponent<Image>();
        inventoryUIPanel.SetActive(false);
        if (draggedItemImage != null)
        {
            draggedItemImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryMode();
        }

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
        if (!isInventoryModeActive)
        {
            DeselectSlot();
        }
    }

    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        Transform itemTransform = clickedSlot.transform.GetChild(1);
        Image itemImage = itemTransform.GetComponent<Image>();

        if (!isDragging)
        {
            if (clickedSlot.GetWeaponInfo() != null)
            {
                draggedWeaponInfo = clickedSlot.GetWeaponInfo();
                clickedSlot.RemoveWeaponInfo();
                clickedSlot.UpdateSlotUI(null, inventoryEmpty, null);

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
            if (clickedSlot.GetWeaponInfo() == null)
            {
                clickedSlot.SetWeaponInfo(draggedWeaponInfo);
                clickedSlot.UpdateSlotUI(null, inventoryEmpty, draggedWeaponInfo.weaponSprite);

                draggedWeaponInfo = null;
                if (draggedItemImage != null)
                {
                    draggedItemImage.gameObject.SetActive(false);
                }
                isDragging = false;
            }
            else
            {
                WeaponInfo temp = clickedSlot.GetWeaponInfo();
                clickedSlot.SetWeaponInfo(draggedWeaponInfo);
                clickedSlot.UpdateSlotUI(null, inventoryEmpty, clickedSlot.GetWeaponInfo().weaponSprite);

                draggedWeaponInfo = temp;
                if (draggedItemImage != null)
                {
                    draggedItemImage.sprite = draggedWeaponInfo.weaponSprite;
                    draggedItemImage.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnTrashCanClicked()
    {
        if (isDragging && draggedWeaponInfo != null)
        {
            draggedWeaponInfo = null;
            if (draggedItemImage != null)
            {
                draggedItemImage.gameObject.SetActive(false);
                draggedItemImage.sprite = null;
            }
            isDragging = false;
        }
        else if (selectedSlot != null)
        {
            selectedSlot.RemoveWeaponInfo();
            Transform itemTransform = selectedSlot.transform.GetChild(1);
            Image itemImage = itemTransform.GetComponent<Image>();
            itemImage.sprite = inventoryEmpty;
            DeselectSlot();
        }
    }

    private void DeselectSlot()
    {
        if (selectedSlot != null)
        {
            HighlightSlot(selectedSlot, false);
            selectedSlot = null;
        }
    }

    private void HighlightSlot(InventorySlot slot, bool highlight)
    {
        // später
    }
}
