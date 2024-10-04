using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateChecker : MonoBehaviour
{
    public WallController wallController;
    private int totalPlates = 3; // Anzahl der Pressure Plates im Spiel
    private int activatedPlates = 0; // Zählt die aktuell aktivierten Pressure Plates

    public void ActivatePlate()
    {
        activatedPlates++;
        CheckAllPlates();
    }

    public void DeactivatePlate()
    {
        activatedPlates--;
    }

    private void CheckAllPlates()
    {
        if (activatedPlates == totalPlates)
        {
            Debug.Log("Alle Druckplatten sind aktiviert.");
            wallController.OpenWall();
        }
        else
        {
            Debug.Log("Nicht alle Druckplatten sind aktiviert.");
        }
    }
}
