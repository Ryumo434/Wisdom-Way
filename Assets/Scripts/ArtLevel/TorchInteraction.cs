using UnityEngine;

public class TorchInteraction : MonoBehaviour
{
    public GameObject flameTorch; // Das GameObject der Flamme
    public GameObject barrier2;   // Das zu deaktivierende Barriere-GameObject
    public GameObject textField;  // Das Textfeld, das aktiviert und deaktiviert wird
    private bool playerInRange = false; // Wird wahr, wenn der Spieler den Collider berührt
    private bool actionTaken = false;   // Verhindert wiederholte Aktivierung
    
    void Start()
    {
        flameTorch.SetActive(false); // Flamme zu Beginn deaktiviert
        textField.SetActive(false);  // Textfeld zu Beginn deaktiviert
    }

    void Update()
    {
        // Wenn der Spieler sich im Trigger-Bereich befindet und die E-Taste gedrückt wird
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !actionTaken)
        {
            flameTorch.SetActive(true);    // Flamme wird aktiviert
            barrier2.SetActive(false);     // Barriere wird deaktiviert
            textField.SetActive(false);    // Textfeld wird deaktiviert
            actionTaken = true;            // Aktion abgeschlossen, verhindert erneutes Auslösen
        }
    }

    // Wird aufgerufen, wenn der Spieler den Collider betritt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !actionTaken)
        {
            playerInRange = true;         // Spieler ist im Bereich
            textField.SetActive(true);    // Textfeld wird angezeigt
        }
    }

    // Wird aufgerufen, wenn der Spieler den Collider verlässt
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !actionTaken)
        {
            playerInRange = false;        // Spieler verlässt den Bereich
            textField.SetActive(false);   // Textfeld wird deaktiviert
        }
    }
}
