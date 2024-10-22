using TMPro;
using UnityEngine;

public class TriggerQuestion : MonoBehaviour
{
    public static TriggerQuestion instance;  // Singleton-Instance

    [SerializeField] public GameObject questionPanel;  // Das Panel, das die Frage und das InputField enthält
    [SerializeField] public GameObject answerInputFieldObject;  // Das TMP_InputField von TextMeshPro
    [SerializeField] public GameObject barrier;  // Das GameObject, das deaktiviert werden soll

    private string correctAnswer = "leonardo da vinci";  // Die richtige Antwort
    private bool isQuestionActive = false;  // Überprüfen, ob die Frage aktiv ist
    [SerializeField] private TMP_InputField answerInputField;  // Das TMP_InputField von TextMeshPro

    private void Start()
    {
        barrier = GameObject.Find("Barrier1");
        // Abonniere das onEndEdit-Event des TMP_InputField
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
            answerInputFieldObject.SetActive(true);
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
            answerInputFieldObject.SetActive(false);
            Time.timeScale = 1f;  // Zeit fortsetzen
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
        // Überprüfen, ob die Escape-Taste gedrückt wird
        if (isQuestionActive && Input.GetKeyDown(KeyCode.Escape))
        {
            // Zeit fortsetzen und das Panel schließen, wenn ESC gedrückt wird
            Time.timeScale = 1f;
            questionPanel.SetActive(false);
            isQuestionActive = false;  // Quiz wird geschlossen
        }
    }

    // Methode wird vom Button aufgerufen, um manuell zu prüfen
    public void CheckAnswer()
    {
        // Bereinige die Eingabe und die richtige Antwort (entferne Leerzeichen und vergleiche case-insensitive)
        string cleanedInput = answerInputField.text.Trim().ToLower().Replace(" ", "");
        string cleanedAnswer = correctAnswer.Trim().ToLower().Replace(" ", "");

        if (cleanedInput == cleanedAnswer)
        {
            // Deaktiviert die Barriere, wenn die Antwort korrekt ist
            barrier.SetActive(false);
            questionPanel.SetActive(false);  // Fragepanel ausblenden
            Time.timeScale = 1f;  // Zeit fortsetzen
            isQuestionActive = false;  // Quiz ist beendet
        }
        else
        {
            // Falsche Eingabe in der Konsole ausgeben
            Debug.Log("Falsche Eingabe: " + answerInputField.text);
            Debug.Log("Falsch");
            // Frage aktiv lassen, damit Spieler mit ESC das Quiz schließen kann
            isQuestionActive = true;  // Quiz bleibt aktiv, damit ESC funktioniert
        }
    }
}
