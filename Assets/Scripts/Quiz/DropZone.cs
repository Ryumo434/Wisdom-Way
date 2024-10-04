using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public string erwarteterTag; // Der Tag der richtigen Antwort

    // Variable, um das zugewiesene Draggable zu speichern
    [HideInInspector]
    public Draggable AssignedDraggable;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject gezogenesObjekt = eventData.pointerDrag;

        if (gezogenesObjekt != null)
        {
            Draggable draggableComponent = gezogenesObjekt.GetComponent<Draggable>();

            if (draggableComponent != null)
            {
                // Falls bereits ein anderes Draggable zugewiesen ist, setze es auf seine Anfangsposition zurück
                if (AssignedDraggable != null && AssignedDraggable != draggableComponent)
                {
                    AssignedDraggable.ResetPosition(); // Setze die Position des vorherigen Draggables zurück
                }

                // Setze das gezogene Objekt als Kind der DropZone
                gezogenesObjekt.transform.SetParent(transform);

                // Setze die Position des gezogenen Objekts auf die der DropZone
                RectTransform draggedRectTransform = gezogenesObjekt.GetComponent<RectTransform>();

                if (draggedRectTransform != null)
                {
                    draggedRectTransform.anchoredPosition = Vector2.zero; // Anker auf die Mitte der DropZone setzen
                }

                // Aktualisiere die Referenzen
                AssignedDraggable = draggableComponent;
                draggableComponent.currentDropZone = this;
            }
        }
    }
}