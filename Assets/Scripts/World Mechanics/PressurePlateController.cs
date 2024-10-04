using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    public string gameObjectName;
    public PressurePlateChecker pressurePlateChecker; // Verweis auf den Manager
    public WallController wallController;
    public int count = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "RobertKoch")
        {
            Debug.Log("Moin, Robert Koch.");
            wallController.OpenWall();
        }
        if (other.name == gameObjectName)
        {
            Debug.Log("Kiste +1");
            addCorrectChest();
        }
        else
        {
            Debug.Log("Falsche Kiste");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "RobertKoch")
        {
            wallController.CloseWall();
        }
        if (other.name == gameObjectName)
        {
            Debug.Log("Kiste -1");
            subtractCorrectChest();
        }
        else
        {
            Debug.Log("Falsche Kiste");
        }
    }

    public void addCorrectChest()
    {
        count++;

        if (count == 1) // Stelle sicher, dass die Kiste nur einmal gezählt wird
        {
            pressurePlateChecker.ActivatePlate();
        }
    }

    public void subtractCorrectChest()
    {
        count--;

        if (count == 0) // Stelle sicher, dass die Kiste nur einmal subtrahiert wird
        {
            pressurePlateChecker.DeactivatePlate();
        }
    }
}