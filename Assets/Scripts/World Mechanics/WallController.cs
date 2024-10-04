using UnityEngine;

public class WallController : MonoBehaviour
{
    public string wallTag; // Der Tag der Wand, die gesteuert werden soll
    public string alignment;
    public bool isOpen = false;
    public float openSpeed = 2f;

    private GameObject wall; // Referenz auf die Wand
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        // Finde die Wand anhand des Tags
        wall = GameObject.FindWithTag(wallTag);

        if (wall == null)
        {
            Debug.LogError("Keine Wand mit dem Tag '" + wallTag + "' gefunden!");
            return;
        }

        closedPosition = wall.transform.position;

        if (alignment == "vertikal")
        {
            openPosition = wall.transform.position - new Vector3(0, 6, 0); // Verschiebe die Wand nach unten
        }
        else if (alignment == "horizontal")
        {
            openPosition = wall.transform.position - new Vector3(3, 0, 0); // Verschiebe die Wand nach links
        }
        else
        {
            Debug.LogError("Ein Fehler ist aufgetreten. '" + alignment + "' ist nicht vordefiniert. Nutze 'horizontal' oder 'vertikal'!");
        }
    }

    void Update()
    {
        if (wall == null) return;

        if (isOpen)
        {
            wall.transform.position = Vector3.Lerp(wall.transform.position, openPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            wall.transform.position = Vector3.Lerp(wall.transform.position, closedPosition, openSpeed * Time.deltaTime);
        }
    }

    public void ToggleWall()
    {
        isOpen = !isOpen;
    }

    public void OpenWall()
    {
        isOpen = true;
    }

    public void CloseWall()
    {
        isOpen = false;
    }
}