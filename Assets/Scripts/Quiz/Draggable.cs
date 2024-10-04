using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [HideInInspector]
    public Transform originalParent;
    [HideInInspector]
    public Vector2 originalPosition;

    // Referenz zur aktuellen DropZone
    [HideInInspector]
    public DropZone currentDropZone;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Finde den übergeordneten Canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas nicht gefunden. Bitte stelle sicher, dass dieses Objekt unter einem Canvas liegt.");
        }
    }

    private void Start()
    {
        // Store the original parent and position at the start
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // Damit das Objekt während des Draggens nicht die Raycasts blockiert

        // Entferne das Draggable aus der aktuellen DropZone, falls vorhanden
        if (currentDropZone != null)
        {
            currentDropZone.AssignedDraggable = null;
            currentDropZone = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Bewegt das Objekt relativ zur Canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Wenn das Objekt nicht auf einer DropZone losgelassen wurde, kehrt es zur ursprünglichen Position zurück
        if (currentDropZone == null)
        {
            ResetPosition();
        }
    }

    // Methode zum Zurücksetzen der Position
    public void ResetPosition()
    {
        // Set the parent first before resetting position
        transform.SetParent(originalParent, false);

        // Reset position relative to the parent
        rectTransform.anchoredPosition = originalPosition;

        currentDropZone = null;
    }
}