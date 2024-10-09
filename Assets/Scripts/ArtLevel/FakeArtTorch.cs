using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FakeArtTorch : MonoBehaviour
{
    public GameObject Torch1;
    public GameObject Torch2;
    public GameObject Torch3;
    public GameObject flameE1; 
    public GameObject flameE2; 
    public GameObject flameE3; 
    public GameObject barrier; // Barriere, die deaktiviert wird, wenn TorchObject3 entfacht wird
    public Image blackBackground; // Bild für den schwarzen Hintergrund, der langsam transparent wird
    private bool playerInRange = false; // Wird wahr, wenn der Spieler den Collider berührt
    private bool actionTaken = false;   // Verhindert wiederholte Aktivierung
    private int torchNumber = 0;
    
    void Start()
    {
        Torch1.SetActive(false); // Flamme zu Beginn deaktiviert
        Torch2.SetActive(false);
        Torch3.SetActive(false);
    }

    void Update()
    {
        // Wenn der Spieler im Trigger-Bereich ist und die E-Taste gedrückt wird
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !actionTaken)
        {
            // Überprüfen, welche Fackel entfacht werden soll
            switch (torchNumber)
            {
                case 1:
                    Torch1.SetActive(true);  // Fackel 1 wird aktiviert
                    falseTourch();
                    break;
                case 2:
                    Torch2.SetActive(true);  // Fackel 2 wird aktiviert
                    falseTourch();
                    break;
                case 3:
                    Torch3.SetActive(true);  // Fackel 3 wird aktiviert
                    trueTorch();
                    break;
                default:
                    Debug.LogWarning("Unbekannte Fackelnummer: " + torchNumber);
                    break;
            }

            actionTaken = true;  // Aktion wurde durchgeführt
            flameE1.SetActive(false);
            flameE2.SetActive(false);
            flameE3.SetActive(false);
        }
    }

    private void falseTourch() 
    {
        Debug.Log("Falsche Fackel");
        StartCoroutine(DeactivateTorchesAfterDelay());
    }

    IEnumerator DeactivateTorchesAfterDelay()
    {
        // 1. Warte 3 Sekunden
        yield return new WaitForSeconds(1f);

        // 2. Fackeln nach 3 Sekunden deaktivieren
        Torch1.SetActive(false);
        Torch2.SetActive(false);
    }

    private void trueTorch()
    {
        barrier.SetActive(false);
        actionTaken = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player") && !actionTaken)
    {
        playerInRange = true;         // Spieler ist im Bereich
        // Unterscheide zwischen den verschiedenen Fackel-Tags
        if (this.CompareTag("torch1"))
        {
            // Logik für torch1
            Debug.Log("Fackel 1");
            flameE1.SetActive(true);
            torchNumber = 1;
        }
        else if (this.CompareTag("torch2"))
        {
            // Logik für torch2
            Debug.Log("Fackel 2");
            flameE2.SetActive(true);
            torchNumber = 2;
        }
        else if (this.CompareTag("torch3"))
        {
            // Logik für torch3
            Debug.Log("Fackel 3");
            flameE3.SetActive(true);
            torchNumber = 3;
            // Weitere Logik für torch3, z.B. Barriere deaktivieren
        }
    }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !actionTaken)
        {
            playerInRange = false;        // Spieler verlässt den Bereich
            flameE1.SetActive(false);
            flameE2.SetActive(false);
            flameE3.SetActive(false);
        }
    }
 
}
