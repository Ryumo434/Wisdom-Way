using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private GameObject Trigger1;
    private bool isPlayerInTrigger = false;
    private bool lastPlayerInTriggerState = false;
   void Update()
    {
        if (isPlayerInTrigger != lastPlayerInTriggerState)
    {
        if (isPlayerInTrigger)
        {
            Debug.Log("True");  // Spieler ist im Trigger
        }
        else
        {
            Debug.Log("False");  // Spieler ist nicht im Trigger
        }

        // Den letzten Zustand aktualisieren
        lastPlayerInTriggerState = isPlayerInTrigger;
    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler den Trigger betritt
        if (other.CompareTag("Player"))
        {
            // Setze die Variable auf True
            
            if (gameObject.CompareTag("Trigger"))
            {
                isPlayerInTrigger = true;
                Debug.Log("triggerEvent");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler den Trigger verlässt
        if (other.CompareTag("Player"))
        {
            // Setze die Variable auf False
            isPlayerInTrigger = false;
        }
    }
    
    //Schreibe hier deine Funktion
}
