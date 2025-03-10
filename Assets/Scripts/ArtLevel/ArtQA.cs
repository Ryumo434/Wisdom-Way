using TMPro;
using UnityEngine;

public class TriggerQuestion : MonoBehaviour
{
    public static TriggerQuestion instance;  // Singleton-Instance

    [SerializeField] private GameObject questionPanel;  // Das Panel, das die Frage und das InputField enthält
    [SerializeField] private GameObject answerInputFieldObject;  // Das TMP_InputField von TextMeshPro
    [SerializeField] private GameObject barrier;  // Das GameObject, das deaktiviert werden soll
    [SerializeField] private GameObject ActivateWeapon;
    [SerializeField] private GameObject PressEMonaLisa;
    [SerializeField] private TMP_InputField answerInputField;  // Das TMP_InputField von TextMeshPro

    private string correctAnswer = "leonardo da vinci";  // Die richtige Antwort
    private bool isQuestionActive = false;  // Überprüfen, ob die Frage aktiv ist
    private bool isQuizSolved = false;  // Überprüfen, ob das Quiz gelöst wurde
    private bool isPlayerInTrigger = false;

    private void Start()
    {
        PressEMonaLisa.SetActive(false);
      // Finde das Root-Objekt "Player"
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            // Finde das Kindobjekt "ActivateWeapon" unter "Player"
            Transform activateWeaponTransform = player.transform.Find("Active Weapon");

            if (activateWeaponTransform != null)
            {
                ActivateWeapon = activateWeaponTransform.gameObject;
            }
            else
            {
                Debug.LogError("ActivateWeapon konnte unter Player nicht gefunden werden!");
            }
        }
        else
        {
            Debug.LogError("Player konnte nicht gefunden werden!");
        }
        barrier = GameObject.Find("Barrier1");
        // Abonniere das onEndEdit-Event des TMP_InputField
        answerInputField.onEndEdit.AddListener(OnSubmit);  
    }

    // Methode wird aufgerufen, wenn ein Collider den Trigger betritt
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler das Tag "Player" hat
        if (other.CompareTag("Player") && !isQuizSolved)
        {
            isPlayerInTrigger = true;
            PressEMonaLisa.SetActive(true);
        }
    }

    private void EisPressed()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isQuizSolved && isPlayerInTrigger) {
            PressEMonaLisa.SetActive(false);
            questionPanel.SetActive(true);
            answerInputFieldObject.SetActive(true);
            Time.timeScale = 0f;  // Zeit anhalten
            ActivateWeapon.SetActive(false);
            isQuestionActive = true;
        }
    }

    // Methode wird aufgerufen, wenn der Collider den Trigger verlässt
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Frage verbergen und Zeit fortsetzen
            isPlayerInTrigger = false;
            PressEMonaLisa.SetActive(false);
            questionPanel.SetActive(false);
            answerInputFieldObject.SetActive(false);
            Time.timeScale = 1f;  // Zeit fortsetzen
            ActivateWeapon.SetActive(true);
            isQuestionActive = false;
        }
    }

    // Diese Methode wird aufgerufen, wenn der Spieler Enter drückt oder das InputField verlässt
    private void OnSubmit(string inputText)
    {
        // Ausgabe des eingegebenen Textes in der Konsole
        Debug.Log("Eingegebener Text: " + inputText);

        // Bereinige die Eingabe und die richtige Antwort (entferne Leerzeichen und vergleiche case-insensitive)
        if (inputText.Trim().ToLower() == correctAnswer.Trim().ToLower())
        {
            // Deaktiviert die Barriere, wenn die Antwort korrekt ist
            barrier.SetActive(false);
            questionPanel.SetActive(false);  // Fragepanel ausblenden
            answerInputFieldObject.SetActive(false);
            Time.timeScale = 1f;  // Zeit fortsetzen
            isQuestionActive = false;  // Quiz ist beendet
            isQuizSolved = true;  // Quiz wurde gelöst
            Debug.Log("Richtige Eingabe: " + inputText);
        }
        else
        {
            // Falsche Eingabe in der Konsole ausgeben
            Debug.Log("Falsche Eingabe: " + inputText);
            Debug.Log("Falsch");
            // Frage aktiv lassen, damit Spieler mit ESC das Quiz schließen kann
            isQuestionActive = true;  // Quiz bleibt aktiv, damit ESC funktioniert
        }
    }

    // Update-Methode, um Eingaben zu erkennen
    private void Update()
    {
        EisPressed();
        // Überprüfen, ob die Escape-Taste gedrückt wird
        if (isQuestionActive && Input.GetKeyDown(KeyCode.Escape))
        {
            // Zeit fortsetzen und das Panel schließen, wenn ESC gedrückt wird
            Time.timeScale = 1f;
            questionPanel.SetActive(false);
            answerInputFieldObject.SetActive(false);
            isQuestionActive = false;  // Quiz wird geschlossen
        }
    }
}