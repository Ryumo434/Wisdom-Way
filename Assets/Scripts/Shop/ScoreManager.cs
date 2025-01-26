using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton-Instanz
    public TMP_Text scoreText; // TextMeshPro-Text für den Score
    private int currentScore = 0; // Aktueller Punktestand

    void Awake()
    {
        // Singleton-Setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objekt zwischen Szenen erhalten
        }
        else
        {
            Destroy(gameObject); // Verhindere Duplikate
        }
    }

    public void AddScore(int amount)
    {
        // Score erhöhen
        currentScore = Mathf.Min(currentScore + amount, 9999);
        UpdateScoreText(currentScore);
    }

    public void UpdateScoreText(int currentScoreVar)
    {
        if (scoreText != null)
        {
            scoreText.text = currentScoreVar.ToString();
        }
        else
        {
            Debug.LogError("ScoreText ist nicht zugewiesen!");
        }
    }

    public int GetScore()
    {
        // Gibt den aktuellen Punktestand zurück
        return currentScore;
    }

    public void SetScore(int newScore)
    {
        // Setzt den aktuellen Score auf einen neuen Wert und aktualisiert die Anzeige
        currentScore = Mathf.Clamp(newScore, 0, 9999);
        UpdateScoreText(currentScore);
    }
}