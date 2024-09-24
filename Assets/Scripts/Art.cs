using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Trigger1;
    [SerializeField] private GameObject Trigger2;
    [SerializeField] private GameObject Trigger3;

    [SerializeField] private GameObject darkBackground;
    [SerializeField] private GameObject MonalisaCap;

    private bool isPlayerInTrigger = false;
    private bool lastPlayerInTriggerState = false;

    // Update is called once per frame
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

    // Wird ausgelöst, wenn der Collider betreten wird
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler den Trigger betritt
        if (other.CompareTag("Player"))
        {
            // Setze die Variable auf True
            darkBackground.SetActive(true);
            
            if (gameObject.CompareTag("Trigger1"))
            {
                isPlayerInTrigger = true;
                MonalisaCap.SetActive(true);
                Debug.Log("trigger1");
            }
            else if (gameObject.CompareTag("Trigger2"))
            {
                isPlayerInTrigger = true;
                Debug.Log("trigger2");
            } 
            else if (gameObject.CompareTag("Trigger3"))
            {
                isPlayerInTrigger = true;
                Debug.Log("trigger3");
            }
        }
    }

    // Wird ausgelöst, wenn der Collider verlassen wird
    private void OnTriggerExit2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler den Trigger verlässt
        if (other.CompareTag("Player"))
        {
            // Setze die Variable auf False
            isPlayerInTrigger = false;
            darkBackground.SetActive(false);
            MonalisaCap.SetActive(false);
        }
    }
}
