using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;  // TextMeshPro Namespace hinzufügen

public class NCPDialog : MonoBehaviour
{
    // Panel, das im Canvas aktiviert werden soll
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject Panel;
    [SerializeField] public TextMeshProUGUI NPCText;

    // Variable zum Speichern, ob der Spieler den Trigger betritt
    private bool isPlayerInTrigger = false;
    private bool lastPlayerInTriggerState = false;
    private int currentTextIndex = 0;  // Index für den aktuellen Text
    private string[] dialoge = new string[]  // Liste der Dialoge
    {
        "Hallo, Reisender!",
        "Ich spüre Energie",
        "Der Turm verbirgt ein Geheimnis...",
        "Finde es heraus!",
    };


    // Update wird einmal pro Frame aufgerufen
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

        // Überprüfen, ob eine beliebige Taste gedrückt wurde
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
            {
                Debug.Log("Gedrückte Taste: " + key);  // Gibt die gedrückte Taste aus
            }
        }
        EisPressed();
    }

     // Methode zur Überprüfung, ob "E" gedrückt wurde
   private void EisPressed()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger)
        {
            // Falls das Panel nicht aktiv ist, aktiviere es und zeige den ersten Text
            if (!Panel.activeInHierarchy)
            {
                Panel.SetActive(true);
                PressE.SetActive(false);
                NPCText.text = dialoge[currentTextIndex];  // Zeigt den ersten Text an
                Debug.Log("Erster Text angezeigt");
                Time.timeScale = 0f;
            }
            else
            {
                // Erhöht den Index und zeigt den nächsten Text, wenn das Panel bereits aktiv ist
                currentTextIndex++;

                // Wenn alle Texte durchlaufen sind, schließt das Panel und setzt den Index zurück
                if (currentTextIndex >= dialoge.Length)
                {
                    Panel.SetActive(false);  // Schließt das Panel
                    currentTextIndex = 0;  // Setzt den Textindex zurück
                    Debug.Log("Dialog beendet, Panel geschlossen.");
                    Time.timeScale = 1f;
                    
                }
                else
                {
                    // Zeigt den nächsten Text an
                    NPCText.text = dialoge[currentTextIndex];
                    Debug.Log("Nächster Text angezeigt");
                }
            }
        }
    }

    // Wird ausgelöst, wenn der Collider betreten wird
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler den Trigger betritt
        if (other.CompareTag("Player"))
        {
            // Setze die Variable auf True
            isPlayerInTrigger = true;

            // Aktiviert das Panel
            PressE.SetActive(true);
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

            // Deaktiviert das Panel
            PressE.SetActive(false);
            Panel.SetActive(false);
            currentTextIndex = 0;
        }
    }
}
