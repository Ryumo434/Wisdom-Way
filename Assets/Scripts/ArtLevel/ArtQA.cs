using TMPro;
using UnityEngine;

public class TriggerQuestion : MonoBehaviour
{
    public GameObject questionPanel;  // Das Panel, das die Frage und das InputField enthält
    public TMP_InputField answerInputField;  // Das TMP_InputField von TextMeshPro
    public GameObject barrier;  // Das GameObject, das deaktiviert werden soll
    private string correctAnswer = "test";  // Die richtige Antwort
    private bool isQuestionActive = false;  // Überprüfen, ob die Frage aktiv ist

    private void Start()
    {
        // Hier abonnieren wir das onEndEdit-Event des TMP_InputField
        answerInputField.onEndEdit.AddListener(OnSubmit);  
    }

    // Methode wird aufgerufen, wenn ein Collider den Trigger betritt
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüfen, ob der Spieler das Tag "Player" hat
        if (other.CompareTag("Player"))
        {
            // Frage anzeigen und Zeit anhalten
            questionPanel.SetActive(true);
            Time.timeScale = 0f;  // Zeit anhalten
            isQuestionActive = true;
        }
    }

    // Methode wird aufgerufen, wenn der Collider den Trigger verlässt
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Frage verbergen und Zeit fortsetzen
            questionPanel.SetActive(false);
            Time.timeScale = 1f;  // Zeit fortsetzen
            isQuestionActive = false;
        }
    }

    // Diese Methode wird vom Button oder bei Eingabe von "Enter" aufgerufen
    private void OnSubmit(string inputText)
    {
        // Ausgabe des eingegebenen Textes in der Konsole
        Debug.Log("Eingegebener Text: " + inputText);
        
        // Optionale Überprüfung, ob die Eingabe korrekt ist
        if (inputText.ToLower() == correctAnswer)
        {
            // Deaktiviert die Barriere, wenn die Antwort korrekt ist
            barrier.SetActive(false);
            questionPanel.SetActive(false);  // Fragepanel ausblenden
            Time.timeScale = 1f;  // Zeit fortsetzen
        }
        else
        {
            Debug.Log("Falsch");
        }
        isQuestionActive = false;  // Frage nicht mehr aktiv
    }

    // Update-Methode, um Eingaben zu erkennen
    private void Update()
    {
        // Überprüfen, ob die Escape-Taste gedrückt wird
        if (isQuestionActive && Input.GetKeyDown(KeyCode.Escape))
        {
            // Zeit fortsetzen und das Panel schließen, wenn ESC gedrückt wird
            Time.timeScale = 1f;
            questionPanel.SetActive(false);
            isQuestionActive = false;
        }
    }

    // Methode wird vom Button aufgerufen, um manuell zu prüfen
    public void CheckAnswer()
    {
        // Vergleicht die Antwort aus dem InputField mit der richtigen Antwort
        if (answerInputField.text.ToLower() == correctAnswer)
        {
            // Deaktiviert die Barriere, wenn die Antwort korrekt ist
            barrier.SetActive(false);
            questionPanel.SetActive(false);  // Fragepanel ausblenden
            Time.timeScale = 1f;  // Zeit fortsetzen
        }
        else
        {
            Debug.Log("Falsch");
        }
        isQuestionActive = false;
    }
}
