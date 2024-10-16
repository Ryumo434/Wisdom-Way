using UnityEngine;
using UnityEngine.UI; // F?r UI-Elemente

public class PressEProgress : MonoBehaviour
{
    public Slider progressBar; // Referenz zum UI-Slider als Fortschrittsanzeige
    public float totalPressTimeRequired = 15f; // Gesamtzeit, die 'e' gedr?ckt werden muss
    public GameObject canvas;

    private bool isPlayerInTrigger = false;
    private float ePressTime = 0f;

    private void Start()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = totalPressTimeRequired;
            progressBar.value = 0f;
            canvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // Verwende OnTriggerEnter2D f?r 2D-Collider
    {
        if (other.CompareTag("Player")) // Annahme: Der Spieler hat den Tag "Player"
        {
            isPlayerInTrigger = true;
            if (progressBar != null)
            {
                canvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Verwende OnTriggerExit2D f?r 2D-Collider
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            ePressTime = 0f; // Fortschritt zur?cksetzen
            if (progressBar != null)
            {
                progressBar.value = 0f;
                canvas.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (Input.GetKey(KeyCode.E))
            {
                // Wenn die Taste gedr?ckt wird, den Fortschritt erh?hen
                ePressTime += Time.deltaTime;
                if (ePressTime >= totalPressTimeRequired)
                {
                    ePressTime = totalPressTimeRequired;
                    // Aktion bei Erreichen der 15 Sekunden
                    Debug.Log("'e' wurde f?r 15 Sekunden gedr?ckt!");
                }
            }
            else
            {
                // Fortschritt zur?cksetzen, wenn 'e' losgelassen wird
                ePressTime = 0f;
            }

            // Fortschrittsanzeige aktualisieren
            if (progressBar != null)
            {
                progressBar.value = ePressTime;
            }
        }
    }
}